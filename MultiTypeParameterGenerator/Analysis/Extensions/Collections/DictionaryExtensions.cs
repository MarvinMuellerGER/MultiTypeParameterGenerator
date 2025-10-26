namespace MultiTypeParameterGenerator.Analysis.Extensions.Collections;

internal static class DictionaryExtensions
{
    internal static IReadOnlyList<IReadOnlyList<(T1, T2)>> GetCombinations<T1, T2>(
        this IDictionary<T1, IReadOnlyList<T2>> lists) =>
        lists.Values.GetCombinations().Select(x => x
                .Select((val, index) => (lists.Keys.ElementAt(index), val))
                .ToList())
            .ToList();

    internal static TValue? GetValueOrDefault<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue = default) =>
        GetValueOrDefault(dictionary, key, out _, defaultValue);

    internal static TValue? GetValueOrDefault<TKey, TValue>(
        this IDictionary<TKey, TValue> dictionary, TKey key, out bool alreadyExisted, TValue? defaultValue = default)
    {
        alreadyExisted = dictionary.TryGetValue(key, out var value);
        return alreadyExisted ? value : defaultValue;
    }
}