using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using MultiTypeParameterGenerator.Analysis.Extensions.Enums;
using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using static Microsoft.CodeAnalysis.TypeKind;
using TypeName = MultiTypeParameterGenerator.Common.Models.TypedValues.TypeName;

namespace MultiTypeParameterGenerator.Common.Factories.Entities;

internal class MethodToOverloadFactory : IMethodToOverloadFactory
{
    public MethodToOverload? Create(IMethodSymbol method)
    {
        if (method.ContainingType is not { } containingType)
            // nothing to do if this method isn't used in a type
            return null;

        var containingTypeKind = GetOverloadingSupportingContainingTypeKindNameOrNull(containingType);
        var generateExtensionMethod = false;
        if (ShouldGenerateExtensionMethod(method, containingTypeKind, ref generateExtensionMethod)) return null;

        return new(generateExtensionMethod,
            GetContainingType(containingTypeKind, containingType),
            GetAccessModifiers(method),
            GetReturnType(method),
            GetMethodName(method),
            GetGenericTypes(method),
            GetAffectedGenericTypes(method),
            GetParameters(method));
    }

    private ContainingType
        GetContainingType(TypeKindName? containingTypeKind, INamedTypeSymbol containingType) =>
        new(containingTypeKind, GetFullContainingTypeName(containingType),
            GetGenericTypesOfContainingType(containingType));

    private static TypeKindName? GetOverloadingSupportingContainingTypeKindNameOrNull(ITypeSymbol containingType)
    {
        if (containingType.IsReadOnly) return null;

        return new(containingType.TypeKind is Interface ? "interface" :
            containingType.IsRecord ? containingType.IsValueType
                ? "record struct"
                : "record" :
            containingType.IsValueType ? "struct" : "class");
    }

    private static bool ShouldGenerateExtensionMethod(IMethodSymbol method, TypeKindName? containingTypeKind,
        ref bool generateExtensionMethod)
    {
        if (containingTypeKind is not null)
            return false;

        var isAccessibleForOtherClass = method.DeclaredAccessibility is Accessibility.Internal
            or Accessibility.ProtectedOrInternal or Accessibility.Public;
        if (!isAccessibleForOtherClass)
            return true;
        generateExtensionMethod = isAccessibleForOtherClass;

        return false;
    }

    private static FullTypeName GetFullContainingTypeName(ITypeSymbol containingType) =>
        new(GetContainingNamespace(containingType), GetContainingTypeName(containingType));

    private static Namespace GetContainingNamespace(ITypeSymbol containingType) =>
        new(containingType.ContainingNamespace.ToString());

    private static TypeName GetContainingTypeName(ITypeSymbol containingType) => new(containingType.Name);

    private static GenericTypeCollection GetGenericTypesOfContainingType(INamedTypeSymbol containingType) =>
        GetGenericTypesOfTypeParameters(containingType.TypeParameters);

    private AccessModifierNameCollection GetAccessModifiers(IMethodSymbol method)
    {
        var accessModifiersAttribute = method.GetAttributes()
            .SingleOrDefault(a => a.AttributeClass?.Name == nameof(AccessModifiersAttribute));
        var accessModifiersEnum = (AccessModifier?)(int?)accessModifiersAttribute?.ConstructorArguments[0].Value;

        List<AccessModifierName> accessModifiers = [];

        if (accessModifiersEnum is not null)
            accessModifiers.Add(new(accessModifiersEnum.Value.GetSourceCode()));

        if (method.IsStatic)
            accessModifiers.Add(new("static"));

        return new(accessModifiers);
    }

    private static ReturnTypeName GetReturnType(IMethodSymbol method) => new(method.ReturnType.ToString());

    private static MethodName GetMethodName(IMethodSymbol method) => new(method.Name);

    private static GenericTypeCollection GetGenericTypes(IMethodSymbol method) =>
        GetGenericTypesOfTypeParameters(method.TypeParameters);

    private static GenericTypeCollection GetGenericTypesOfTypeParameters(
        IReadOnlyList<ITypeParameterSymbol> typeParameters) =>
        new(typeParameters.SelectToReadonlyList(tp => new GenericType(new(tp.ToString()),
            new(new(tp.ConstraintTypes.SelectWithIndexToReadonlyList(tuple =>
                    new TypeName(tuple.Item.Name +
                                 (tp.ConstraintNullableAnnotations[tuple.Index] is NullableAnnotation.Annotated
                                     ? "?"
                                     : string.Empty)))),
                tp.HasValueTypeConstraint,
                tp.HasReferenceTypeConstraint,
                tp.ReferenceTypeConstraintNullableAnnotation is NullableAnnotation.Annotated,
                tp.HasNotNullConstraint,
                tp.HasUnmanagedTypeConstraint,
                tp.HasConstructorConstraint))));

    private AcceptedTypesForAffectedGenericTypeCollection GetAffectedGenericTypes(IMethodSymbol method) =>
        new(method.TypeParameters.Select(typeParameter => new AcceptedTypesForAffectedGenericType(
                new(new(typeParameter.Name)), GetAcceptedTypes(typeParameter)))
            .WhereToReadonlyList(affectedGenericType => affectedGenericType.AcceptedTypes.Values.Any()));

    private AcceptedTypeCollection GetAcceptedTypes(ITypeParameterSymbol typeParameter)
    {
        var (types, asGenericTypes) = GetAcceptedTypesAttributeInformation(typeParameter);
        return new(types.SelectToReadonlyList(tp => GetAcceptedType(tp, asGenericTypes)));
    }

    private AcceptedTypesAttributeInformation GetAcceptedTypesAttributeInformation(ITypeParameterSymbol type)
    {
        var acceptedTypesAttribute = type.GetAttributes()
            .SingleOrDefault(a => a.AttributeClass?.Name == nameof(AcceptedTypesAttribute));

        var types = acceptedTypesAttribute?.AttributeClass?.TypeArguments ?? [];

        var shortConstructor = acceptedTypesAttribute?.ConstructorArguments.Length is 1;
        var asGenericTypes =
            !shortConstructor && (bool)(acceptedTypesAttribute?.ConstructorArguments[0].Value ?? false);

        var additionalTypeNames = acceptedTypesAttribute?.ConstructorArguments[shortConstructor ? 0 : 1].Values
            .Select(typedConstant => (string)typedConstant.Value!) ?? [];
        var additionalTypes = additionalTypeNames
            .Select(typeName => GetTypeSymbolByName(type.ContainingType, typeName))
            .WhereNotNull();

        return new(types.Select(t => new TypeInformation(t, false)).Concat(additionalTypes).ToImmutableArray(),
            asGenericTypes);
    }

    /// <summary>
    ///     Searches for a type symbol with the specified name in the context of the provided type symbol.
    /// </summary>
    /// <param name="contextTypeSymbol">The type symbol that provides the context for the search</param>
    /// <param name="typeName">The qualified name of the type to find</param>
    /// <returns>The found type symbol or null</returns>
    private static TypeInformation? GetTypeSymbolByName(
        ITypeSymbol contextTypeSymbol, string typeName)
    {
        var typeNameParts = typeName.TrimEnd('?').Split('.');

        // Search through contexts from inner to outer (starting with the current type)
        INamespaceOrTypeSymbol currentContext = contextTypeSymbol;
        while (currentContext is not null)
        {
            if (TryFindTypeInCurrentContext(currentContext, typeNameParts, out var foundType))
                return foundType is null ? null : new(foundType, typeName.EndsWith("?"));

            // No matches in the current context, move to the containing namespace
            if (currentContext is INamespaceSymbol { IsGlobalNamespace: true })
                return null; // Reached global namespace without success

            currentContext = currentContext.ContainingNamespace;
        }

        return null;

        // Local helper function to search for a type in the current context
        static bool TryFindTypeInCurrentContext(
            INamespaceOrTypeSymbol context,
            string[] typeNameParts,
            out ITypeSymbol? foundType)
        {
            foundType = null;
            var potentialMatch = context;

            foreach (var namePart in typeNameParts)
            {
                var nestedType = potentialMatch.GetTypeMembers(namePart).FirstOrDefault();
                if (nestedType is null)
                    return false; // Path is broken, no match

                potentialMatch = nestedType;
            }

            // If we found something that is different from the original context
            if (SymbolEqualityComparer.Default.Equals(potentialMatch, context) ||
                potentialMatch is not ITypeSymbol resultType)
                return false;

            foundType = resultType;
            return true;
        }
    }

    private static AcceptedType GetAcceptedType(TypeInformation typeInformation, bool asGenericType)
    {
        var type = typeInformation.Type;

        if (IsGenericType(type))
        {
            asGenericType = true;
            type = ((INamedTypeSymbol)type).TypeArguments[0];
        }

        return new(new(type.ToString()), typeInformation.IsNullable,
            asGenericType && type is { IsReferenceType: true, IsSealed: false });
    }

    private static bool IsGenericType(ITypeSymbol type) =>
        Regex.IsMatch(type.ToString(), @"MultiTypeParameterGenerator\.GenericType<([\w\.]+)>");

    private static ParameterCollection GetParameters(IMethodSymbol method) =>
        new(method.Parameters.SelectToReadonlyList(p =>
            new Parameter(new(p.Type.ToString()), new(p.Name))));

    private sealed record TypeInformation(ITypeSymbol Type, bool IsNullable);

    private readonly record struct AcceptedTypesAttributeInformation(
        IReadOnlyList<TypeInformation> Types,
        bool AsGenericTypes);
}