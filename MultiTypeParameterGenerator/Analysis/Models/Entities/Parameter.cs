using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using Type = MultiTypeParameterGenerator.Common.Models.Entities.Type;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal readonly record struct Parameter(
    Type Type,
    ParameterName Name,
    bool IsOptional = false,
    object? ExplicitDefaultValue = null)
{
    internal SourceCode TypeNameForSourceCode { get; init; } = new(Type.ShortenedSourceCode.Value);

    internal SourceCode SourceCode => new($"{TypeNameForSourceCode} {Name}{DefaultValueCode}");

    private SourceCode DefaultValueCode => IsOptional
        ? new($" = {ExplicitDefaultValue ?? (Type.IsNullable && Type is not GenericTypeParameter ? "null" : "default")}")
        : new();
}