namespace MultiTypeParameterGenerator.Generation.Extensions.Collections;

internal static class EnumerableExtensions
{
    internal static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action) =>
        source.ToList().ForEach(action);

    internal static IReadOnlyList<TSource> ToReadonlyList<TSource>(this IEnumerable<TSource> source) =>
        source.ToList();

    internal static IEnumerable<(T Item, bool IsFirst)> WithIsFirst<T>(this IEnumerable<T> source) =>
        source.WithIndex().Select(tuple => (tuple.Item, tuple.Index is 0));

    private static IEnumerable<(T Item, int Index)> WithIndex<T>(this IEnumerable<T> source)
    {
        var index = 0;
        foreach (var item in source)
            yield return (item, index++);
    }
}