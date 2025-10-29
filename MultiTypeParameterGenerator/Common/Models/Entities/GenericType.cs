using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal sealed record GenericType(GenericTypeName Name, TypeConstraint? Constraint = null, bool IsNullable = false)
{
    internal SourceCode? ConstraintSourceCode =>
        Constraint?.SourceCode is null
            ? null
            : new(
                $"""

                       where {Name} : {Constraint.SourceCode}
                 """);

    private SourceCode NameInclNullableAnnotation => new($"{Name}{NullableAnnotation}");

    private SourceCode NullableAnnotation => IsNullable ? new("?") : new();

    public override string ToString() => NameInclNullableAnnotation.ToString();

    internal static GenericType FromAcceptedType(AcceptedType acceptedType, bool isNullable, bool useFullTypeNames) =>
        new(new(acceptedType.TypeNameForSourceCode.Value),
            acceptedType.UseTypeConstraint
                ? new(new(useFullTypeNames
                    ? acceptedType.Type.SourceCode.Value
                    : acceptedType.Type.ShortenedSourceCode.Value))
                : null,
            isNullable);
}