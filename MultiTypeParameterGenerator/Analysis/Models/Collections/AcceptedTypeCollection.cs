using MultiTypeParameterGenerator.Analysis.Models.Entities;

namespace MultiTypeParameterGenerator.Analysis.Models.Collections;

internal sealed record AcceptedTypeCollection(params IReadOnlyList<AcceptedType> Values);