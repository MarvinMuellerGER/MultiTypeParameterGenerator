namespace MultiTypeParameterGenerator.Common.Models.TypedValues;

internal sealed record TypeName(string Value)
{
    internal TypeName WithoutContainingType => new(Value.Split('.').Last());

    public override string ToString() => Value;
}