namespace MultiTypeParameterGenerator.Common.Models.TypedValues;

internal sealed record SourceCode(string Value)
{
    internal SourceCode() : this(string.Empty) { }

    public override string ToString() => Value;

    public static SourceCode operator +(SourceCode left, SourceCode right) => new(left.Value + right.Value);
}