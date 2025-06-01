using Microsoft.CodeAnalysis;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Common.Extensions.Roslyn;

internal static class SourceProductionContextExtensions
{
    internal static void AddSource(
        this SourceProductionContext sourceProductionContext, SourceCodeFile sourceCodeFile) =>
        sourceProductionContext.AddSource(sourceCodeFile.FileName.Value, sourceCodeFile.SourceCode.Value);
}