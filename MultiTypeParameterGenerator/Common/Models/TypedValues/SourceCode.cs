namespace MultiTypeParameterGenerator.Common.Models.TypedValues;

public sealed record SourceCode(string Value)
{
    public SourceCode() : this(string.Empty)
    {
    }

    public override string ToString() => Value;

    public static SourceCode operator +(SourceCode left, SourceCode right) => new(left.Value + right.Value);
}