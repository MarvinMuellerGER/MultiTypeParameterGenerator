namespace MultiTypeParameterGenerator.Common.Models.TypedValues;

internal sealed record GenericTypeName(string Value)
{
    public override string ToString() => Value;
}