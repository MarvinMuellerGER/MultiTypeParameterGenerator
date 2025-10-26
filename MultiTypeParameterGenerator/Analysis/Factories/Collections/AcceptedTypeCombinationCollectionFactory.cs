using MultiTypeParameterGenerator.Analysis.Extensions.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Analysis.Factories.Collections;

using TypeCombinationItem = (GenericType GenericType, AcceptedType AcceptedType);

internal sealed class AcceptedTypeCombinationCollectionFactory : IAcceptedTypeCombinationCollectionFactory
{
    public AcceptedTypeCombinationCollection Create(MethodToOverload methodToOverload)
    {
        var typeCombinations = GetTypeCombinations(methodToOverload);
        var globalTracker = new GlobalTypeCountTracker();

        // First pass: create combinations with type indices
        var combinations = typeCombinations
            .Select(combination => CreateAcceptedTypeCombination(combination, globalTracker))
            .ToList();

        // Second pass: assign indices for combinations where all types use constraints
        return new(AssignConstraintCombinationIndices(combinations));
    }

    private static IEnumerable<IReadOnlyList<TypeCombinationItem>> GetTypeCombinations(
        MethodToOverload methodToOverload)
    {
        var acceptedTypesByGenericType = methodToOverload.AffectedGenericTypes.Values.ToDictionary(
            affectedGenericType => affectedGenericType.AffectedGenericType,
            affectedGenericType => affectedGenericType.AcceptedTypes.Values);

        return acceptedTypesByGenericType.GetCombinations();
    }

    private static AcceptedTypeCombination CreateAcceptedTypeCombination(
        IReadOnlyList<TypeCombinationItem> combination,
        GlobalTypeCountTracker globalTracker)
    {
        var combinationTracker = new CombinationTypeCountTracker(combination);

        var acceptedTypesForGenericTypes = combination.SelectToReadonlyList(item =>
            CreateAcceptedTypeForAffectedGenericType(item, combination, globalTracker, combinationTracker));

        return new(acceptedTypesForGenericTypes);
    }

    private static AcceptedTypeForAffectedGenericType CreateAcceptedTypeForAffectedGenericType(
        TypeCombinationItem item,
        IReadOnlyList<TypeCombinationItem> combination,
        GlobalTypeCountTracker globalTracker,
        CombinationTypeCountTracker combinationTracker)
    {
        var (genericType, acceptedType) = item;
        var shortTypeName = acceptedType.Type.ShortenedSourceCodeWithoutContainingType;

        var contextKey = CreateContextKey(combination, item);
        var isFirstOccurrence =
            globalTracker.TrackOccurrence(contextKey, shortTypeName, acceptedType.UseTypeConstraint);

        if (acceptedType.UseTypeConstraint) combinationTracker.IncrementCount(shortTypeName);

        var updatedAcceptedType = acceptedType with
        {
            IndexOfParametersWithSameType = combinationTracker.GetCount(shortTypeName),
            IndexOfParametersWithSameShortTypeName = CalculateShortTypeNameIndex(
                combination,
                globalTracker.GetContextCounts(contextKey),
                shortTypeName)
        };

        return new(genericType, updatedAcceptedType, !isFirstOccurrence);
    }

    private static int CalculateShortTypeNameIndex(
        IReadOnlyList<TypeCombinationItem> combination,
        Dictionary<SourceCode, int> contextCounts,
        SourceCode shortTypeName)
    {
        var hasMultipleTypesWithSameName = contextCounts.Count > 1;
        var hasMultipleTypeConstraints = combination.Count(item => item.AcceptedType.UseTypeConstraint) > 1;

        return hasMultipleTypesWithSameName || hasMultipleTypeConstraints
            ? contextCounts.GetValueOrDefault(shortTypeName)
            : 0;
    }

    private static IReadOnlyList<AcceptedTypeCombination> AssignConstraintCombinationIndices(
        IEnumerable<AcceptedTypeCombination> combinations)
    {
        var constraintCombinationIndex = 0;

        return combinations.SelectToReadonlyList(combination =>
        {
            if (!combination.AllUseTypeConstraints)
                return combination;

            constraintCombinationIndex++;
            return combination with
            {
                IndexOfCombinationsWhereAllUseTypeConstraints = constraintCombinationIndex
            };
        });
    }

    private static string CreateContextKey(IReadOnlyList<TypeCombinationItem> combination, TypeCombinationItem item)
    {
        var otherTypes = combination
            .Except([item])
            .Select(other => other.AcceptedType.Type.ShortenedSourceCodeExclNullableAnnotation.Value)
            .Join();

        return $"{item.GenericType}:{otherTypes.GetHashCode()}";
    }

    /// <summary>
    ///     Tracks type counts across all combinations for a specific context (generic type + other types in combination).
    /// </summary>
    private sealed class GlobalTypeCountTracker
    {
        private readonly Dictionary<string, Dictionary<SourceCode, int>> _countsByContext = new();

        public bool TrackOccurrence(string contextKey, SourceCode shortTypeName, bool useTypeConstraint)
        {
            if (!_countsByContext.TryGetValue(contextKey, out var counts))
            {
                counts = new();
                _countsByContext[contextKey] = counts;
            }

            var isFirstOccurrence = counts.Count == 0;

            if (useTypeConstraint) counts[shortTypeName] = counts.GetValueOrDefault(shortTypeName) + 1;

            return isFirstOccurrence;
        }

        public Dictionary<SourceCode, int> GetContextCounts(string contextKey) =>
            _countsByContext.TryGetValue(contextKey, out var counts) ? counts : new();
    }

    /// <summary>
    ///     Tracks type counts within a single combination to calculate same-type parameter indices.
    /// </summary>
    private sealed class CombinationTypeCountTracker
    {
        private readonly Dictionary<SourceCode, int> _counts = new();

        public CombinationTypeCountTracker(IReadOnlyList<TypeCombinationItem> combination)
        {
            // Pre-calculate counts for all types with constraints in this combination
            foreach (var (_, acceptedType) in combination)
            {
                if (!acceptedType.UseTypeConstraint)
                    continue;

                var shortTypeName = acceptedType.Type.ShortenedSourceCodeWithoutContainingType;
                _counts[shortTypeName] = 0;
            }
        }

        public void IncrementCount(SourceCode shortTypeName)
        {
            _counts[shortTypeName] = _counts.GetValueOrDefault(shortTypeName) + 1;
        }

        public int GetCount(SourceCode shortTypeName) => _counts.GetValueOrDefault(shortTypeName);
    }
}