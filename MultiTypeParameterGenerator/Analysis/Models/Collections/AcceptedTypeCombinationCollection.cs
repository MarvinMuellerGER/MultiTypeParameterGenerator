using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Analysis.Models.Collections;

internal sealed record AcceptedTypeCombinationCollection(params IReadOnlyList<AcceptedTypeCombination> Values);