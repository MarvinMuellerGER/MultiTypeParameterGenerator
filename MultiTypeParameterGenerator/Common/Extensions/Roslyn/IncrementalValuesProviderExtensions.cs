using Microsoft.CodeAnalysis;

namespace MultiTypeParameterGenerator.Common.Extensions.Roslyn;

internal static class IncrementalValuesProviderExtensions
{
    internal static IncrementalValuesProvider<T> ConcatAll<T>(
        this IEnumerable<IncrementalValuesProvider<T>> providers) =>
        providers.Aggregate((list, next) => list.Concat(next));

    private static IncrementalValuesProvider<T> Concat<T>(
        this IncrementalValuesProvider<T> provider1, IncrementalValuesProvider<T> provider2) =>
        provider1.Collect()
            .Combine(provider2.Collect())
            .SelectMany(static (item, _) => item.Left.Concat(item.Right));
}