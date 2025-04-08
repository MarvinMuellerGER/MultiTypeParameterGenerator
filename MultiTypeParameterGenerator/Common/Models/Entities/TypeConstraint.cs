using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal sealed record TypeConstraint(
    TypeNameCollection Types,
    bool HasValueTypeConstraint,
    bool HasReferenceTypeConstraint,
    bool HasNullableReferenceTypeConstraint,
    bool HasNotNullConstraint,
    bool HasUnmanagedTypeConstraint,
    bool HasConstructorConstraint)
{
    internal TypeConstraint(TypeName type) : this(new(type), false, false, false, false, false, false)
    {
    }

    internal SourceCode? SourceCode => Constraints.Any() ? new(Constraints.Join()) : null;

    private IReadOnlyList<SourceCode> Constraints =>
        new[]
        {
            Types.SourceCode,
            HasValueTypeConstraint ? new("struct") : null,
            HasReferenceTypeConstraint
                ? new($"class{(HasNullableReferenceTypeConstraint ? "?" : string.Empty)}")
                : null,
            HasNotNullConstraint ? new("notnull") : null,
            HasUnmanagedTypeConstraint ? new("unmanaged") : null,
            HasConstructorConstraint ? new("new()") : null
        }.WhereNotNull();
}