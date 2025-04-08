using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal readonly record struct Parameter(ParameterTypeName Type, ParameterName Name)
{
    internal SourceCode TypeNameForSourceCode { get; init; } = new(Type.ShortName.Value);

    internal SourceCode SourceCode => new($"{TypeNameForSourceCode} {Name}");
}