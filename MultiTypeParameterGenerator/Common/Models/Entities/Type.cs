using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal abstract record Type(bool IsNullable)
{
    internal abstract FullTypeNameCollection FullTypeNames { get; }

    internal virtual SourceCode SourceCode => new($"{SourceCodeExclNullableAnnotation}{NullableAnnotationSourceCode}");
    internal abstract SourceCode SourceCodeExclNullableAnnotation { get; }

    internal virtual SourceCode ShortenedSourceCode =>
        new($"{ShortenedSourceCodeExclNullableAnnotation}{NullableAnnotationSourceCode}");

    internal abstract SourceCode ShortenedSourceCodeExclNullableAnnotation { get; }
    internal abstract SourceCode ShortenedSourceCodeWithoutContainingType { get; }

    private SourceCode NullableAnnotationSourceCode => new(IsNullable ? "?" : string.Empty);

    public override string ToString() => SourceCode.Value;

    internal Type WithAcceptedTypes(AcceptedTypeCombination acceptedTypeCombination) =>
        this switch
        {
            NamedType namedType => namedType with
            {
                Name = namedType.Name.WithAcceptedTypes(acceptedTypeCombination),
                TypeArguments = namedType.TypeArguments.WithAcceptedTypes(acceptedTypeCombination)
            },
            ArrayType arrayType => arrayType with
            {
                ElementType = arrayType.ElementType.WithAcceptedTypes(acceptedTypeCombination)
            },
            TupleType tupleType => tupleType with
            {
                ContainedTypes = tupleType.ContainedTypes.WithAcceptedTypes(acceptedTypeCombination)
            },
            GenericTypeParameter genericTypeParameter =>
                WithAcceptedTypes(genericTypeParameter, acceptedTypeCombination),
            _ => this
        };

    private static Type WithAcceptedTypes(
        GenericTypeParameter genericTypeParameter, AcceptedTypeCombination acceptedTypeCombination)
    {
        var nameWithAcceptedTypes = genericTypeParameter.Name.WithAcceptedTypes(acceptedTypeCombination);

        return AcceptedTypeIsNamedType(acceptedTypeCombination, genericTypeParameter)
            ? new NamedType(nameWithAcceptedTypes, genericTypeParameter.IsNullable)
            : genericTypeParameter with { Name = nameWithAcceptedTypes };
    }

    private static bool AcceptedTypeIsNamedType(AcceptedTypeCombination acceptedTypeCombination,
        GenericTypeParameter genericTypeParameter) =>
        acceptedTypeCombination.Values
            .FirstOrDefault(acceptedType =>
                acceptedType.AffectedGenericType.Name.Value == genericTypeParameter.Name.TypeName.Value)
            ?.AcceptedType.Type is NamedType;
}

internal sealed record NamedType(
    FullTypeName Name,
    NamedType? ContainingType,
    TypeCollection TypeArguments,
    bool IsNullable) : Type(IsNullable)
{
    internal NamedType(FullTypeName Name, NamedType? ContainingType = null) :
        this(Name, ContainingType, new(), false) { }

    internal NamedType(FullTypeName Name, bool IsNullable) :
        this(Name, null, new(), IsNullable) { }

    internal override FullTypeNameCollection FullTypeNames =>
        TypeArguments.FullTypeNames.ConcatDistinct(Name);

    internal TypeName FullNameWithGenericTypesCount =>
        new(Name + (TypeArguments.Values.Count is 0 ? string.Empty : $"`{TypeArguments.Values.Count}"));

    internal override SourceCode SourceCodeExclNullableAnnotation =>
        GetSourceCode(ContainingType?.SourceCodeExclNullableAnnotation, Name.Value, TypeArguments.SourceCode);

    internal override SourceCode ShortenedSourceCodeExclNullableAnnotation =>
        GetSourceCode(ContainingType?.ShortenedSourceCodeExclNullableAnnotation, Name.TypeName.Value,
            TypeArguments.ShortenedSourceCode);

    internal override SourceCode ShortenedSourceCodeWithoutContainingType =>
        GetSourceCode(null, Name.TypeName.Value, TypeArguments.ShortenedSourceCode);

    public override string ToString() => base.ToString();

    private static SourceCode GetSourceCode(
        SourceCode? containingTypeName, string name, SourceCode typeArgumentsSourceCode) =>
        new(
            $"{GetTypeArgumentsSourceCode(containingTypeName)}{name}{GetContainedTypesSourceCode(typeArgumentsSourceCode)}");

    private static SourceCode GetTypeArgumentsSourceCode(SourceCode? containingTypeName) =>
        new(containingTypeName is null ? string.Empty : $"{containingTypeName}.");

    private static SourceCode GetContainedTypesSourceCode(SourceCode typeArgumentsSourceCode) =>
        new(typeArgumentsSourceCode.Value.Any() ? $"<{typeArgumentsSourceCode}>" : string.Empty);
}

internal sealed record ArrayType(Type ElementType) : Type(ElementType.IsNullable)
{
    internal override FullTypeNameCollection FullTypeNames => ElementType.FullTypeNames;

    internal override SourceCode SourceCode => GetSourceCode(ElementType.SourceCode);

    internal override SourceCode SourceCodeExclNullableAnnotation =>
        GetSourceCode(ElementType.SourceCodeExclNullableAnnotation);

    internal override SourceCode ShortenedSourceCode => GetSourceCode(ElementType.ShortenedSourceCode);

    internal override SourceCode ShortenedSourceCodeExclNullableAnnotation =>
        GetSourceCode(ElementType.ShortenedSourceCodeExclNullableAnnotation);

    internal override SourceCode ShortenedSourceCodeWithoutContainingType =>
        GetSourceCode(ElementType.ShortenedSourceCodeWithoutContainingType);

    public override string ToString() => base.ToString();

    private static SourceCode GetSourceCode(SourceCode elementTypeSourceCode) => new($"{elementTypeSourceCode}[]");
}

internal sealed record TupleType(TypeCollection ContainedTypes, bool IsNullable) : Type(IsNullable)
{
    internal override FullTypeNameCollection FullTypeNames => ContainedTypes.FullTypeNames;

    internal override SourceCode SourceCodeExclNullableAnnotation => GetSourceCode(ContainedTypes.SourceCode);

    internal override SourceCode ShortenedSourceCodeExclNullableAnnotation =>
        GetSourceCode(ContainedTypes.ShortenedSourceCode);

    internal override SourceCode ShortenedSourceCodeWithoutContainingType =>
        GetSourceCode(ContainedTypes.ShortenedSourceCodeWithoutContainingType);

    public override string ToString() => base.ToString();

    private static SourceCode GetSourceCode(SourceCode containedTypesSourceCode) =>
        new($"({containedTypesSourceCode})");
}

internal sealed record GenericTypeParameter(FullTypeName Name, bool IsNullable = false) : Type(IsNullable)
{
    internal override FullTypeNameCollection FullTypeNames { get; } = new(Name);
    internal override SourceCode SourceCodeExclNullableAnnotation => new(Name.TypeName.Value);
    internal override SourceCode ShortenedSourceCodeExclNullableAnnotation => SourceCodeExclNullableAnnotation;
    internal override SourceCode ShortenedSourceCodeWithoutContainingType => SourceCodeExclNullableAnnotation;

    public override string ToString() => base.ToString();
}