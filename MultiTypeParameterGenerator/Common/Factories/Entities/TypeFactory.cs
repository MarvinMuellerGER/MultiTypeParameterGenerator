using Microsoft.CodeAnalysis;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using Type = MultiTypeParameterGenerator.Common.Models.Entities.Type;

namespace MultiTypeParameterGenerator.Common.Factories.Entities;

internal sealed class TypeFactory : ITypeFactory
{
    private INamedTypeSymbol _containingTypeOfMethod = null!;

    public Type Create(ITypeSymbol type) =>
        type switch
        {
            INamedTypeSymbol namedTypeSymbol => type.IsTupleType
                ? CreateTupleType(namedTypeSymbol)
                : CreateNamedType(namedTypeSymbol),
            IArrayTypeSymbol arrayTypeSymbol => CreateArrayType(arrayTypeSymbol),
            ITypeParameterSymbol typeParameterSymbol => CreateGenericType(typeParameterSymbol),
            _ => throw new NotSupportedException($"Unsupported type: {type.GetType().Name} ({type})")
        };

    public void SetContainingTypeOfMethod(INamedTypeSymbol containingTypeOfMethod) =>
        _containingTypeOfMethod = containingTypeOfMethod;

    private TupleType CreateTupleType(INamedTypeSymbol type) =>
        new(CreateTypeCollection(type.TupleElements.SelectToReadonlyList(te => te.Type)),
            GetNullableAnnotation(type));

    private NamedType CreateNamedType(INamedTypeSymbol type) =>
        new(GetFullTypeName(type), GetContainingType(type), CreateTypeCollection(type.TypeArguments), GetNullableAnnotation(type));

    private ArrayType CreateArrayType(IArrayTypeSymbol type) => new(Create(type.ElementType));

    private static GenericTypeParameter CreateGenericType(ITypeParameterSymbol type) =>
        new(GetFullTypeName(type), GetNullableAnnotation(type));

    private NamedType? GetContainingType(INamedTypeSymbol type) =>
        type.ContainingType is null ||
        SymbolEqualityComparer.Default.Equals(type.ContainingType, _containingTypeOfMethod)
            ? null
            : CreateNamedType(type.ContainingType);

    private static FullTypeName GetFullTypeName(ITypeSymbol type) =>
        type.SpecialType is SpecialType.None
            ? new(type.ContainingType is null ? GetNamespace(type) : null, new(type.Name))
            : new(null, new(type.ToString()));

    private static Namespace? GetNamespace(ITypeSymbol type) =>
        type.ContainingNamespace is null ? null : new(type.ContainingNamespace.ToString());

    private TypeCollection CreateTypeCollection(IReadOnlyList<ITypeSymbol> types) =>
        new(types.SelectToReadonlyList(Create));

    private static bool GetNullableAnnotation(ITypeSymbol type) =>
        type.NullableAnnotation is NullableAnnotation.Annotated;
}