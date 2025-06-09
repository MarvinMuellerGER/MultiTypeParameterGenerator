namespace MultiTypeParameterGenerator.Common.Extensions;

internal static class StringExtensions
{
    internal static string Remove(this string value, string? valueToRemove) =>
        valueToRemove is null ? value : value.Replace(valueToRemove, string.Empty);

    internal static string RemoveMultiple<T>(this string value, IReadOnlyList<T> valuesToRemove) where T : notnull =>
        valuesToRemove.Aggregate(value,
            (current, t) => current.Replace(t.ToString(), string.Empty));

    internal static string ReplaceMultiple(
        this string value, IReadOnlyList<char> valuesToReplace, Func<char, string> newValueFunc) =>
        valuesToReplace.Aggregate(value,
            (current, t) => current.Replace(t.ToString(), newValueFunc(t)));
}