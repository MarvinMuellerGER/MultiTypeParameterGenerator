using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Common.Models.Collections;

internal sealed record FullTypeNameCollection(params IReadOnlyList<FullTypeName> Values)
{
    internal FullTypeNameCollection(IEnumerable<FullTypeName> values) : this(values.ToList()) { }

    internal FullTypeNameCollection WithUniqueTypeNames => new(Values.FilterUniqueBy(t => t.TypeName));

    internal FullTypeNameCollection WithoutUniqueTypeNames => new(Values.Except(WithUniqueTypeNames.Values));

    internal FullTypeNameCollection ConcatDistinct(FullTypeNameCollection other) => ConcatDistinct(other.Values);

    internal FullTypeNameCollection ConcatDistinct(FullTypeName other) => ConcatDistinct([other]);

    private FullTypeNameCollection ConcatDistinct(IReadOnlyList<FullTypeName> other) =>
        new(Values.ConcatDistinct(other).Distinct());
}