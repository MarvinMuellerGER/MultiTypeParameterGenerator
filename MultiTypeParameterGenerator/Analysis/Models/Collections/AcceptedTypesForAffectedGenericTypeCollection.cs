using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.Collections;

namespace MultiTypeParameterGenerator.Analysis.Models.Collections;

internal sealed record AcceptedTypesForAffectedGenericTypeCollection(
    params IReadOnlyList<AcceptedTypesForAffectedGenericType> Values)
{
    internal FullTypeNameCollection FullTypeNames => new(Values.SelectMany(p => p.AcceptedTypes.FullTypeNames.Values));
}