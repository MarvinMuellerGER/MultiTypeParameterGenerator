namespace MultiTypeParameterGenerator.Common.Models.TypedValues;

internal sealed record TypeName(string Value)
{
    public override string ToString() => Value;
}