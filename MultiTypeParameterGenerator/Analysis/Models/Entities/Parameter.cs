using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal readonly record struct Parameter(FullTypeName Type, ParameterName Name)
{
    internal SourceCode TypeNameForSourceCode { get; init; } = new(Type.TypeName.Value);

    internal SourceCode SourceCode => new($"{TypeNameForSourceCode} {Name}");
}