using MultiTypeParameterGenerator.Analysis.Extensions.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Analysis.Factories.Collections;

using Key = (GenericType AffectedGenericType, int OtherAcceptedTypesHashCode);
using TypeCombinationItem = (GenericType GenericType, AcceptedType AcceptedType);

internal sealed class AcceptedTypeCombinationCollectionFactory : IAcceptedTypeCombinationCollectionFactory
{
    public AcceptedTypeCombinationCollection Create(MethodToOverload methodToOverload)
    {
        var typeCombinations = GetTypeCombinations(methodToOverload);
        var countByShortNameOfAcceptedTypeByAffectedGenericType = new Dictionary<Key, Dictionary<SourceCode, int>>();
        var countByAllUseTypeConstraints = 0;

        var combinations = typeCombinations
            .Select(combination => CreateAcceptedTypeCombination(
                combination,
                countByShortNameOfAcceptedTypeByAffectedGenericType))
            .SelectToReadonlyList(combination => TrackAllTypeConstraintsCombinations(
                combination,
                ref countByAllUseTypeConstraints));

        return new(combinations);
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
        IReadOnlyList<TypeCombinationItem> acceptedTypeList,
        Dictionary<Key, Dictionary<SourceCode, int>> countByShortNameOfAcceptedTypeByAffectedGenericType)
    {
        var countByGenericAcceptedType = new Dictionary<SourceCode, int>();

        var acceptedTypesForGenericTypes = acceptedTypeList.SelectToReadonlyList(item =>
            CreateAcceptedTypeForAffectedGenericType(
                item,
                acceptedTypeList,
                countByShortNameOfAcceptedTypeByAffectedGenericType,
                countByGenericAcceptedType));

        return new(acceptedTypesForGenericTypes);
    }

    private static AcceptedTypeForAffectedGenericType CreateAcceptedTypeForAffectedGenericType(
        TypeCombinationItem item,
        IReadOnlyList<TypeCombinationItem> acceptedTypeList,
        Dictionary<Key, Dictionary<SourceCode, int>> countByShortNameOfAcceptedTypeByAffectedGenericType,
        Dictionary<SourceCode, int> countByGenericAcceptedType)
    {
        var (genericType, acceptedType) = item;
        var key = CreateContextKey(acceptedTypeList, item);

        var countByShortNameOfAcceptedType = countByShortNameOfAcceptedTypeByAffectedGenericType
            .GetValueOrDefault(key, out var isNotFirstOccurrence, new())!;

        var shortAcceptedTypeName = acceptedType.Type.ShortenedSourceCodeWithoutContainingType;

        if (acceptedType.UseTypeConstraint)
        {
            UpdateTypeCounts(
                countByShortNameOfAcceptedType,
                countByGenericAcceptedType,
                shortAcceptedTypeName);

            countByShortNameOfAcceptedTypeByAffectedGenericType[key] = countByShortNameOfAcceptedType;
        }

        var updatedAcceptedType = UpdateAcceptedTypeIndices(
            acceptedType,
            shortAcceptedTypeName,
            acceptedTypeList,
            countByShortNameOfAcceptedType,
            countByGenericAcceptedType);

        return new(
            genericType,
            updatedAcceptedType,
            isNotFirstOccurrence);
    }

    private static void UpdateTypeCounts(
        Dictionary<SourceCode, int> countByShortNameOfAcceptedType,
        Dictionary<SourceCode, int> countByGenericAcceptedType,
        SourceCode shortAcceptedTypeName)
    {
        countByShortNameOfAcceptedType[shortAcceptedTypeName] =
            countByShortNameOfAcceptedType.GetValueOrDefault(shortAcceptedTypeName) + 1;

        countByGenericAcceptedType[shortAcceptedTypeName] =
            countByGenericAcceptedType.GetValueOrDefault(shortAcceptedTypeName) + 1;
    }

    private static AcceptedType UpdateAcceptedTypeIndices(
        AcceptedType acceptedType,
        SourceCode shortAcceptedTypeName,
        IReadOnlyList<TypeCombinationItem> acceptedTypeList,
        Dictionary<SourceCode, int> countByShortNameOfAcceptedType,
        Dictionary<SourceCode, int> countByGenericAcceptedType)
    {
        var indexOfParametersWithSameType = countByGenericAcceptedType.GetValueOrDefault(shortAcceptedTypeName);
        var indexOfParametersWithSameShortTypeName = CalculateShortTypeNameIndex(
            acceptedTypeList,
            countByShortNameOfAcceptedType,
            shortAcceptedTypeName);

        return acceptedType with
        {
            IndexOfParametersWithSameType = indexOfParametersWithSameType,
            IndexOfParametersWithSameShortTypeName = indexOfParametersWithSameShortTypeName
        };
    }

    private static int CalculateShortTypeNameIndex(
        IReadOnlyList<TypeCombinationItem> acceptedTypeList,
        Dictionary<SourceCode, int> countByShortNameOfAcceptedType,
        SourceCode shortAcceptedTypeName)
    {
        var hasMultipleTypesWithSameName = countByShortNameOfAcceptedType.Count > 1;
        var hasMultipleTypeConstraints = acceptedTypeList.Count(item => item.AcceptedType.UseTypeConstraint) > 1;

        return hasMultipleTypesWithSameName || hasMultipleTypeConstraints
            ? countByShortNameOfAcceptedType.GetValueOrDefault(shortAcceptedTypeName)
            : 0;
    }

    private static AcceptedTypeCombination TrackAllTypeConstraintsCombinations(
        AcceptedTypeCombination acceptedTypeCombination,
        ref int countByAllUseTypeConstraints)
    {
        if (!acceptedTypeCombination.AllUseTypeConstraints)
            return acceptedTypeCombination;

        countByAllUseTypeConstraints++;
        return acceptedTypeCombination with
        {
            IndexOfCombinationsWhereAllUseTypeConstraints = countByAllUseTypeConstraints
        };
    }

    private static Key CreateContextKey(
        IReadOnlyList<TypeCombinationItem> acceptedTypeList,
        TypeCombinationItem item)
    {
        var otherAcceptedTypesHashCode = acceptedTypeList
            .Except([item])
            .Select(otherItem => otherItem.AcceptedType.Type.ShortenedSourceCodeExclNullableAnnotation.Value)
            .Join()
            .GetHashCode();

        return (item.GenericType, otherAcceptedTypesHashCode);
    }
}