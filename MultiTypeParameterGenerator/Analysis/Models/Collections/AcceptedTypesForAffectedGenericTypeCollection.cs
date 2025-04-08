using MultiTypeParameterGenerator.Analysis.Extensions.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Extensions.Collections;

namespace MultiTypeParameterGenerator.Analysis.Models.Collections;

internal sealed record AcceptedTypesForAffectedGenericTypeCollection
{
    public AcceptedTypesForAffectedGenericTypeCollection(
        params IReadOnlyList<AcceptedTypesForAffectedGenericType> Values)
    {
        var countByShortTypeName = new Dictionary<AcceptedTypeName, (AcceptedTypeName FullName, int Count)>();

        var acceptedTypes = Values.SelectMany(v => v.AcceptedTypes.Values).DistinctToReadonlyList();
        acceptedTypes = acceptedTypes.SelectToReadonlyList(acceptedType =>
        {
            var fullNameAndCount =
                countByShortTypeName.GetValueOrDefault(acceptedType.ShortName, new(acceptedType.Name, 0));

            if (fullNameAndCount.Count is 0 || fullNameAndCount.FullName != acceptedType.Name)
                countByShortTypeName[acceptedType.ShortName] =
                    fullNameAndCount with { Count = fullNameAndCount.Count + 1 };

            return acceptedType with
            {
                IndexOfAcceptedTypesWithSameShortName = countByShortTypeName[acceptedType.ShortName].Count
            };
        });

        this.Values = Values.SelectToReadonlyList(v => v with
        {
            AcceptedTypes = new(v.AcceptedTypes.Values.SelectToReadonlyList(acceptedType => acceptedTypes.First(a =>
                acceptedType == a with
                {
                    IndexOfAcceptedTypesWithSameShortName = 0
                })))
        });
    }

    public IReadOnlyList<AcceptedTypesForAffectedGenericType> Values { get; }
}