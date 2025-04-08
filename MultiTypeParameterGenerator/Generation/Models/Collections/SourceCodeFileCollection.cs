using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Generation.Models.Collections;

internal sealed record SourceCodeFileCollection(params IReadOnlyList<SourceCodeFile> Values);