namespace MultiTypeParameterGenerator.Analysis.Models.TypedValues;

internal sealed record ParameterName(string Value)
{
    public override string ToString() => Value;
}