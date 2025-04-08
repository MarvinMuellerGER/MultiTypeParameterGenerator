namespace MultiTypeParameterGenerator.Analysis.Models.TypedValues;

internal sealed record AcceptedTypeName(string Value)
{
    internal AcceptedTypeName ShortName => new(Value.Split('.').LastOrDefault() ?? Value);

    public override string ToString() => Value;
}