using System.Collections.Immutable;

namespace MultiTypeParameterGenerator.Common.Extensions.Collections;

internal static class EnumerableExtensions
{
    internal static void ReplaceWhere<TSource>(
        this List<TSource> list, Predicate<TSource> predicate, TSource newValue)
    {
        var index = list.FindIndex(predicate);
        if (index is -1) return;

        list[index] = newValue;
    }

    internal static IReadOnlyList<TSource> WhereNotNull<TSource>(this IEnumerable<TSource?> enumerable) =>
        enumerable.Where(i => i is not null).ToList()!;

    internal static IReadOnlyList<TSource> WhereToReadonlyList<TSource>(
        this IEnumerable<TSource> enumerable, Func<TSource, bool> predicate) =>
        enumerable.Where(predicate).ToList();

    internal static IReadOnlyList<TResult> SelectToReadonlyList<TSource, TResult>(
        this IEnumerable<TSource> enumerable, Func<TSource, TResult> selector) =>
        enumerable.Select(selector).ToList();

    internal static IReadOnlyList<TResult> SelectWithIndexToReadonlyList<TSource, TResult>(
        this IEnumerable<TSource> enumerable, Func<(int Index, TSource Item), TResult> selector) =>
        enumerable.Index().Select(selector).ToList();

    internal static IReadOnlyList<TResult> SelectManyToReadonlyList<TSource, TResult>(
        this IEnumerable<TSource> enumerable, Func<TSource, IEnumerable<TResult>> selector) =>
        enumerable.SelectMany(selector).ToList();

    internal static ImmutableHashSet<TResult> SelectToImmutableHashSet<T, TResult>(
        this IEnumerable<T> enumerable, Func<T, TResult> selector) =>
        enumerable.Select(selector).ToImmutableHashSet();

    internal static IReadOnlyList<TSource> DistinctToReadonlyList<TSource>(this IEnumerable<TSource> enumerable) =>
        enumerable.Distinct().ToList();

    internal static string Join<T>(this IEnumerable<T> source, string separator = ", ") =>
        string.Join(separator, source);

    private static IEnumerable<(int Index, TSource Item)> Index<TSource>(this IEnumerable<TSource> source)
    {
        var index = -1;
        foreach (var element in source)
        {
            index++;

            yield return (index, element);
        }
    }
}