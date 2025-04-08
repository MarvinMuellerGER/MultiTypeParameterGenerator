using MultiTypeParameterGenerator.Analysis.Extensions.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Analysis.Factories.Collections;

using Key = (GenericType AffectedGenericType, int OtherAcceptedTypesHashCode);

internal class AcceptedTypeCombinationCollectionFactory : IAcceptedTypeCombinationCollectionFactory
{
    public AcceptedTypeCombinationCollection Create(MethodToOverload methodToOverload)
    {
        var isNotFirstGenericAcceptedTypeByAffectedGenericType = new Dictionary<Key, bool>();

        return new(methodToOverload.AffectedGenericTypes.Values.ToDictionary(
                affectedGenericType => affectedGenericType.AffectedGenericType,
                affectedGenericType => affectedGenericType.AcceptedTypes.Values).GetCombinations()
            .SelectToReadonlyList(acceptedTypeList =>
            {
                var countByGenericAcceptedType = new Dictionary<AcceptedTypeName, int>();

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

                            countByGenericAcceptedType[acceptedType.Name] =
                                countByGenericAcceptedType.GetValueOrDefault(acceptedType.Name) + 1;
                        }

                        return new AcceptedTypeForAffectedGenericType(
                            affectedGenericType, isNotFirstGenericAcceptedType,
                            acceptedType with
                            {
                                IndexOfParametersWithSameType =
                                countByGenericAcceptedType.GetValueOrDefault(acceptedType.Name)
                            });
                    }));
            }));
    }

    private static Key GetKey(
        IReadOnlyList<(GenericType, AcceptedType)> acceptedTypeList, (GenericType, AcceptedType) item) =>
        (item.Item1, acceptedTypeList.Except([item])
            .Select(otherAcceptedType => otherAcceptedType.Item2.ShortName).Join().GetHashCode());
}