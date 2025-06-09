using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Collections;

internal sealed record NamespaceCollection(params IReadOnlyList<Namespace> Values);