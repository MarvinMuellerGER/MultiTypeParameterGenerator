using MultiTypeParameterGenerator.Analysis.Extensions.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Analysis.Factories.Collections;

using Key = (GenericType AffectedGenericType, int OtherAcceptedTypesHashCode);

internal sealed class AcceptedTypeCombinationCollectionFactory : IAcceptedTypeCombinationCollectionFactory
{
    public AcceptedTypeCombinationCollection Create(MethodToOverload methodToOverload)
    {
        var countByShortNameOfAcceptedTypeByAffectedGenericType = new Dictionary<Key, Dictionary<SourceCode, int>>();
        var countByAllUseTypeConstraints = 0;

        return new(methodToOverload.AffectedGenericTypes.Values.ToDictionary(
                affectedGenericType => affectedGenericType.AffectedGenericType,
                affectedGenericType => affectedGenericType.AcceptedTypes.Values).GetCombinations()
            .Select(acceptedTypeList =>
            {
                var countByGenericAcceptedType = new Dictionary<SourceCode, int>();

                return new AcceptedTypeCombination(
                    acceptedTypeList.SelectToReadonlyList(item =>
                    {
                        var affectedGenericType = item.Item1;
                        var acceptedType = item.Item2;

                        var key = GetKey(acceptedTypeList, item);
                        var countByShortNameOfAcceptedType =
                            countByShortNameOfAcceptedTypeByAffectedGenericType.GetValueOrDefault(
                                key, out var isNotFirstGenericAcceptedType, new())!;

                        var shortAcceptedTypeName = acceptedType.Type.ShortenedSourceCodeWithoutContainingType;
                        if (acceptedType.UseTypeConstraint)
                        {
                            countByShortNameOfAcceptedType[shortAcceptedTypeName] =
                                countByShortNameOfAcceptedType.GetValueOrDefault(shortAcceptedTypeName) + 1;

                            countByShortNameOfAcceptedTypeByAffectedGenericType[key] = countByShortNameOfAcceptedType;
                            
                            countByGenericAcceptedType[shortAcceptedTypeName] =
                                countByGenericAcceptedType.GetValueOrDefault(shortAcceptedTypeName) + 1;
                        }

                        return new AcceptedTypeForAffectedGenericType(
                            affectedGenericType,
                            acceptedType with
                            {
                                IndexOfParametersWithSameType =
                                countByGenericAcceptedType.GetValueOrDefault(shortAcceptedTypeName),

                                IndexOfParametersWithSameShortTypeName =
                                countByShortNameOfAcceptedType.Count > 1 ||
                                acceptedTypeList.Count(a => a.Item2.UseTypeConstraint) > 1
                                    ? countByShortNameOfAcceptedType.GetValueOrDefault(shortAcceptedTypeName)
                                    : 0
                            }, isNotFirstGenericAcceptedType);
                    }));
            }).SelectToReadonlyList(acceptedTypeCombination =>
            {
                if (!acceptedTypeCombination.AllUseTypeConstraints)
                    return acceptedTypeCombination;

                countByAllUseTypeConstraints++;
                return acceptedTypeCombination with
                {
                    IndexOfCombinationsWhereAllUseTypeConstraints = countByAllUseTypeConstraints
                };
            }));
    }

    private static Key GetKey(
        IReadOnlyList<(GenericType, AcceptedType)> acceptedTypeList, (GenericType, AcceptedType) item) =>
        (item.Item1, acceptedTypeList.Except([item])
            .Select(otherAcceptedType => otherAcceptedType.Item2.Type.ShortenedSourceCodeExclNullableAnnotation.Value)
            .Join().GetHashCode());
}