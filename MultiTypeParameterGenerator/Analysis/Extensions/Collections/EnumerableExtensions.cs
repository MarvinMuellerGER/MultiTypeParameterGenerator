namespace MultiTypeParameterGenerator.Analysis.Extensions.Collections;

internal static class EnumerableExtensions
{
    internal static IReadOnlyList<TSource> ReplaceAll<TSource>(
        this IEnumerable<TSource> enumerable, Func<TSource, TSource> newValueFunc)
    {
        var list = enumerable.ToList();
        list.ReplaceAll(newValueFunc);
        return list;
    }

    internal static IReadOnlyList<IReadOnlyList<T>> GetCombinations<T>(this IEnumerable<IEnumerable<T>> lists) =>
        GetCombinations(lists.Select(l => l.ToList()).ToList(), 0, []).ToList();

    private static void ReplaceAll<TSource>(this List<TSource> list, Func<TSource, TSource> newValueFunc) =>
        list.ForEachWithModification(value => list.Replace(value, newValueFunc));

    private static void ForEachWithModification<TSource>(this List<TSource> list, Action<TSource> action)
    {
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < list.Count; i++)
            action(list[i]);
    }

    private static void Replace<TSource>(
        this List<TSource> list, TSource valueToReplace, Func<TSource, TSource> newValueFunc)
    {
        var index = list.IndexOf(valueToReplace);
        list[index] = newValueFunc(list[index]);
    }

    private static IEnumerable<IReadOnlyList<T>> GetCombinations<T>(
        IReadOnlyList<IReadOnlyList<T>> lists, int depth, List<T> current)
    {
        if (depth == lists.Count)
        {
            yield return [..current];
            yield break;
        }

        foreach (var item in lists[depth])
        {
            current.Add(item);
            foreach (var combination in GetCombinations(lists, depth + 1, current))
                yield return combination;
            current.RemoveAt(current.Count - 1);
        }
    }
}