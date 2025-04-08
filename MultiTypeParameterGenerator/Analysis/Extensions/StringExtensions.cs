namespace MultiTypeParameterGenerator.Analysis.Extensions;

internal static class StringExtensions
{
    internal static string ReplaceMultiple(
        this string value, IReadOnlyList<char> valuesToReplace, Func<char, string> newValueFunc) =>
        valuesToReplace.Aggregate(value,
            (current, t) => current.Replace(t.ToString(), newValueFunc(t)));
}