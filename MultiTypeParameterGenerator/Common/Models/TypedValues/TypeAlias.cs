namespace MultiTypeParameterGenerator.Common.Models.TypedValues;

internal sealed record TypeAlias(string Value)
{
    public override string ToString() => Value;
}