using Microsoft.CodeAnalysis;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Models.Collections;

namespace MultiTypeParameterGenerator.Common.Extensions.Roslyn;

internal static class SourceProductionContextExtensions
{
    internal static void AddSources(
        this SourceProductionContext sourceProductionContext, SourceCodeFileCollection sourceCodeFiles)
    {
        foreach (var sourceCodeFile in sourceCodeFiles.Values)
            sourceProductionContext.AddSource(sourceCodeFile);
    }

    private static void AddSource(
        this SourceProductionContext sourceProductionContext, SourceCodeFile sourceCodeFile) =>
        sourceProductionContext.AddSource(sourceCodeFile.FileName.Value, sourceCodeFile.SourceCode.Value);
}