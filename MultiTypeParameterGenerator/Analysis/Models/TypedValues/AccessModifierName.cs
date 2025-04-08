namespace MultiTypeParameterGenerator.Analysis.Models.TypedValues;

internal sealed record AccessModifierName(string Value)
{
    public override string ToString() => Value;
}