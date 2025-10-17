using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using Type = MultiTypeParameterGenerator.Common.Models.Entities.Type;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal readonly record struct Parameter(Type Type, ParameterName Name)
{
    internal SourceCode TypeNameForSourceCode { get; init; } = new(Type.ShortenedSourceCode.Value);

    internal SourceCode SourceCode => new($"{TypeNameForSourceCode} {Name}");
}