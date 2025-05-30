using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Common.Extensions.Roslyn;

internal static class SyntaxValueProviderExtensions
{
    internal static IncrementalValuesProvider<T> ForPolyGenericAttributeWithMetadataName<T>(
        this SyntaxValueProvider syntaxValueProvider,
        string fullyQualifiedMetadataName,
        Func<SyntaxNode, CancellationToken, bool> predicate,
        Func<GeneratorAttributeSyntaxContext, CancellationToken, T> transform,
        int minGenericTypesCount = 2,
        int maxGenericTypesCount = 10) =>
        syntaxValueProvider.ForAttributesWithMetadataNames(GetGenericAttributeNames(
            fullyQualifiedMetadataName, minGenericTypesCount, maxGenericTypesCount), predicate, transform);

    private static IncrementalValuesProvider<T> ForAttributesWithMetadataNames<T>(
        this SyntaxValueProvider syntaxValueProvider,
        ImmutableHashSet<string> fullyQualifiedMetadataNames,
        Func<SyntaxNode, CancellationToken, bool> predicate,
        Func<GeneratorAttributeSyntaxContext, CancellationToken, T> transform) =>
        fullyQualifiedMetadataNames.Select(fullyQualifiedMetadataName =>
                syntaxValueProvider.ForAttributeWithMetadataName(fullyQualifiedMetadataName, predicate, transform))
            .ConcatAll();

    private static ImmutableHashSet<string> GetGenericAttributeNames(
        string attributeName, int minGenericTypesCount, int maxGenericTypesCount) =>
        Enumerable.Range(minGenericTypesCount, maxGenericTypesCount)
            .SelectToImmutableHashSet(i => $"{attributeName}`{i}");
}