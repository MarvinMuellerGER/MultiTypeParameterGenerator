namespace MultiTypeParameterGenerator.Analysis.Models.TypedValues;

internal sealed record MethodName(string Value)
{
    public override string ToString() => Value;
}