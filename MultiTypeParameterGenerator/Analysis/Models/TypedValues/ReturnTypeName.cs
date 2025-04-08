namespace MultiTypeParameterGenerator.Analysis.Models.TypedValues;

internal sealed record ReturnTypeName(string Value) : TypeName(Value)
{
    public override string ToString() => Value;
}