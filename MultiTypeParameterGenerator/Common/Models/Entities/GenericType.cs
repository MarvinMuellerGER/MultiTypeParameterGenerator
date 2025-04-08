using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal sealed record GenericType(GenericTypeName Name, TypeConstraint? Constraint = null)
{
    internal SourceCode? ConstraintSourceCode =>
        Constraint?.SourceCode is null
            ? null
            : new(
                $"""

                       where {Name} : {Constraint.SourceCode}
                 """);

    public override string ToString() => Name.ToString();

    public static GenericType FromAcceptedType(AcceptedType acceptedType) =>
        new(new(acceptedType.TypeNameForSourceCode.Value),
            acceptedType.UseTypeConstraint ? new(new(acceptedType.NameInclNullableAnnotation.Value)) : null);
}