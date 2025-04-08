namespace MultiTypeParameterGenerator.Common.Models.TypedValues;

internal sealed record TypeKindName(string Value)
{
    public override string ToString() => Value;
}