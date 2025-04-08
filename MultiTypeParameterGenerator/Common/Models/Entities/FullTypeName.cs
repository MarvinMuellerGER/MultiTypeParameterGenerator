using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal sealed record FullTypeName(Namespace Namespace, TypeName TypeName)
{
    internal string Value { get; } = $"{Namespace}.{TypeName}";

    public override string ToString() => Value;
}