using Microsoft.CodeAnalysis;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Models.Collections;

namespace MultiTypeParameterGenerator.Common.Extensions.Roslyn;

internal static class IncrementalGeneratorPostInitializationContextExtensions
{
    internal static void AddSources(
        this IncrementalGeneratorPostInitializationContext sourceProductionContext,
        SourceCodeFileCollection sourceCodeFiles)
    {
        foreach (var sourceCodeFile in sourceCodeFiles.Values)
            sourceProductionContext.AddSource(sourceCodeFile);
    }

    private static void AddSource(
        this IncrementalGeneratorPostInitializationContext sourceProductionContext, SourceCodeFile sourceCodeFile) =>
        sourceProductionContext.AddSource(sourceCodeFile.FileName.Value, sourceCodeFile.SourceCode.Value);
}