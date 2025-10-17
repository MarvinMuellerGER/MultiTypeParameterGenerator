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
        var isNotFirstGenericAcceptedTypeByAffectedGenericType = new Dictionary<Key, bool>();
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

                        var isNotFirstGenericAcceptedType =
                            isNotFirstGenericAcceptedTypeByAffectedGenericType.GetValueOrDefault(key);

                        if (acceptedType.UseTypeConstraint)
                        {
                            isNotFirstGenericAcceptedTypeByAffectedGenericType[key] = true;

                            countByGenericAcceptedType[acceptedType.Type.ShortenedSourceCodeWithoutContainingType] =
                                countByGenericAcceptedType.GetValueOrDefault(acceptedType.Type.ShortenedSourceCodeWithoutContainingType) + 1;
                        }

                        return new AcceptedTypeForAffectedGenericType(
                            affectedGenericType, isNotFirstGenericAcceptedType,
                            acceptedType with
                            {
                                IndexOfParametersWithSameType =
                                countByGenericAcceptedType.GetValueOrDefault(acceptedType.Type.ShortenedSourceCodeWithoutContainingType)
                            });
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
            .Select(otherAcceptedType => otherAcceptedType.Item2.ShortName.Value).Join().GetHashCode());
}