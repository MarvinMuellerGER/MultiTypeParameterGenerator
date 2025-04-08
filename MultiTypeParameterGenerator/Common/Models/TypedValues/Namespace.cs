namespace MultiTypeParameterGenerator.Common.Models.TypedValues;

internal sealed record Namespace(string Value)
{
    public override string ToString() => Value;
}