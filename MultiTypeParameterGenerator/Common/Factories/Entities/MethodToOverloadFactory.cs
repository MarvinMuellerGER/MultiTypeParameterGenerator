using System.Collections.Immutable;
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
using static MultiTypeParameterGenerator.Common.Extensions.StringExtensions;
using FullTypeName = MultiTypeParameterGenerator.Common.Models.Entities.FullTypeName;
using TypeName = MultiTypeParameterGenerator.Common.Models.TypedValues.TypeName;

namespace MultiTypeParameterGenerator.Common.Factories.Entities;

internal sealed class MethodToOverloadFactory : IMethodToOverloadFactory
{
    public MethodToOverload? Create(IMethodSymbol method)
    {
        if (method.ContainingType is not { } containingType)
            // nothing to do if this method isn't used in a type
            return null;

        var useFullTypeNames = method.ContainingAssembly.GetAttributes()
            .Any(a => a.AttributeClass?.Name is nameof(UseFullTypeNamesAttribute));

        var containingTypeKind = GetOverloadingSupportingContainingTypeKindNameOrNull(containingType);
        var generateExtensionMethod = false;
        if (ShouldGenerateExtensionMethod(method, containingTypeKind, ref generateExtensionMethod)) return null;

        return new(useFullTypeNames,
            generateExtensionMethod,
            GetContainingType(containingTypeKind, containingType),
            GetAccessModifiers(method),
            GetReturnType(method),
            GetMethodName(method),
            GetGenericTypes(method),
            GetAffectedGenericTypes(method),
            GetParameters(method));
    }

    private static ContainingType
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

    private static bool ShouldGenerateExtensionMethod(
        IMethodSymbol method, TypeKindName? containingTypeKind, ref bool generateExtensionMethod)
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

    private static GenericTypeCollection GetGenericTypesOfContainingType(INamedTypeSymbol containingType) =>
        GetGenericTypesOfTypeParameters(containingType.TypeParameters);

    private static AccessModifierNameCollection GetAccessModifiers(IMethodSymbol method)
    {
        var accessModifiersAttribute = method.GetAttributes()
            .SingleOrDefault(a => a.AttributeClass?.Name is nameof(AccessModifiersAttribute));
        var accessModifiersEnum = (AccessModifier?)(int?)accessModifiersAttribute?.ConstructorArguments[0].Value;

        List<AccessModifierName> accessModifiers = [];

        if (accessModifiersEnum is not null)
            accessModifiers.Add(new(accessModifiersEnum.Value.GetSourceCode()));

        if (method.IsStatic)
            accessModifiers.Add(new("static"));

        return new(accessModifiers);
    }

    private static FullTypeName GetReturnType(IMethodSymbol method) => GetFullTypeName(method.ReturnType);

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

    private static AcceptedTypesForAffectedGenericTypeCollection GetAffectedGenericTypes(IMethodSymbol method) =>
        new(method.TypeParameters.Select(typeParameter => new AcceptedTypesForAffectedGenericType(
                new(new(typeParameter.Name)), GetAcceptedTypes(typeParameter)))
            .WhereToReadonlyList(affectedGenericType => affectedGenericType.AcceptedTypes.Values.Any()));

    private static AcceptedTypeCollection GetAcceptedTypes(ITypeParameterSymbol typeParameter)
    {
        var (types, asGenericTypes) = GetAcceptedTypesAttributeInformation(typeParameter);
        return new(types.SelectToReadonlyList(tp => GetAcceptedType(tp, asGenericTypes)));
    }

    private static AcceptedTypesAttributeInformation GetAcceptedTypesAttributeInformation(ITypeParameterSymbol type)
    {
        var acceptedTypesAttribute = type.GetAttributes()
            .SingleOrDefault(a => a.AttributeClass?.Name is nameof(AcceptedTypesAttribute));

        var types = (acceptedTypesAttribute?.AttributeClass?.TypeArguments ?? []).ToList();

        var acceptedTypeCollections = types.WhereToReadonlyList(IsAcceptedTypeCollection);
        var typesFromCollections = acceptedTypeCollections.SelectMany(t => GetTypeArguments(t));
        types.RemoveAll(acceptedTypeCollections);
        types.AddRange(typesFromCollections);

        var shortConstructor = acceptedTypesAttribute?.ConstructorArguments.Length is 1;
        var asGenericTypes =
            !shortConstructor && (bool)(acceptedTypesAttribute?.ConstructorArguments[0].Value ?? false);

        var additionalTypeNames = acceptedTypesAttribute?.ConstructorArguments[shortConstructor ? 0 : 1].Values
            .Select(typedConstant => (string)typedConstant.Value!) ?? [];
        var additionalTypes = additionalTypeNames
            .Select(typeName => GetTypeSymbolByName(type.ContainingType, typeName))
            .WhereNotNull();

        return new([..types.Select(t => new TypeInformation(t, false)).Concat(additionalTypes)],
            asGenericTypes);
    }

    /// <summary>
    ///     Searches for a type symbol with the specified name in the context of the provided type symbol.
    /// </summary>
    /// <param name="contextTypeSymbol">The type symbol that provides the context for the search</param>
    /// <param name="typeName">The qualified name of the type to find</param>
    /// <returns>The found type symbol or null</returns>
    private static TypeInformation? GetTypeSymbolByName(ITypeSymbol contextTypeSymbol, string typeName)
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
            type = GetTypeArguments(type)[0];
        }

        return new(GetFullTypeName(type), typeInformation.IsNullable,
            asGenericType && type is { IsReferenceType: true, IsSealed: false });
    }

    private static bool IsAcceptedTypeCollection(ITypeSymbol type) =>
        type.ContainingNamespace?.Name == typeof(AcceptedTypesCollection<,>).Namespace &&
        type.Name is nameof(AcceptedTypesCollection<,>);

    private static bool IsGenericType(ITypeSymbol type) =>
        type.ContainingNamespace?.Name == typeof(GenericType).Namespace && type.Name is nameof(GenericType);

    private static ParameterCollection GetParameters(IMethodSymbol method) => new(
        method.Parameters.SelectToReadonlyList(p => new Parameter(GetFullTypeName(p.Type), new(p.Name))));

    private static FullTypeName GetFullTypeName(ITypeSymbol type)
    {
        var @namespace = GetNamespace(type);
        if (@namespace is not null && !type.ToString().Contains(@namespace.Value))
            @namespace = null;

        return new(@namespace, GetTypeName(type, @namespace));
    }

    private static FullTypeName GetFullContainingTypeName(ITypeSymbol type) =>
        new(GetNamespace(type), GetTypeName(type));

    private static Namespace? GetNamespace(ITypeSymbol type) =>
        type.ContainingNamespace is null ? null : new(type.ContainingNamespace.ToString());

    private static TypeName GetTypeName(ITypeSymbol type, Namespace? @namespace) =>
        new(type.ToString().Remove($"{@namespace?.Value}."));

    private static TypeName GetTypeName(ITypeSymbol type) => new(type.Name);

    private static ImmutableArray<ITypeSymbol> GetTypeArguments(ITypeSymbol typeSymbol) =>
        ((INamedTypeSymbol)typeSymbol).TypeArguments;

    private sealed record TypeInformation(ITypeSymbol Type, bool IsNullable);

    private readonly record struct AcceptedTypesAttributeInformation(
        IReadOnlyList<TypeInformation> Types,
        bool AsGenericTypes);
}