using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Generation.Models.Collections;

internal sealed record MethodToOverloadCollection(params IReadOnlyList<MethodToOverload> Values);