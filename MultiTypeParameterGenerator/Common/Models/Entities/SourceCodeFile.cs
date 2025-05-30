using MultiTypeParameterGenerator.Common.Models.TypedValues;
using MultiTypeParameterGenerator.Generation.Models.TypedValues;
using static MultiTypeParameterGenerator.Common.Utils.Constants;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal readonly record struct SourceCodeFile(FileName FileName, SourceCode SourceCode)
{
    internal SourceCode SourceCode { get; } = SourceGeneratedFileHeader + SourceCode;
}