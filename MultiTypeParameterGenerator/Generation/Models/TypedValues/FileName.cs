namespace MultiTypeParameterGenerator.Generation.Models.TypedValues;

internal sealed record FileName(string Value)
{
    public override string ToString() => Value;
}