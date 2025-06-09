using MultiTypeParameterGenerator.Analysis.Extensions.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Analysis.Models.Collections;

internal sealed record AcceptedTypesForAffectedGenericTypeCollection
{
    internal AcceptedTypesForAffectedGenericTypeCollection(
        params IReadOnlyList<AcceptedTypesForAffectedGenericType> Values)
    {
        var countByShortTypeName = new Dictionary<TypeName, (FullTypeName FullName, int Count)>();

        var acceptedTypes = Values.SelectMany(v => v.AcceptedTypes.Values).DistinctToReadonlyList();
        acceptedTypes = acceptedTypes.SelectToReadonlyList(acceptedType =>
        {
            var fullNameAndCount =
                countByShortTypeName.GetValueOrDefault(acceptedType.Name.TypeName.WithoutContainingType,
                    new(acceptedType.Name, 0));

            if (fullNameAndCount.Count is 0 || fullNameAndCount.FullName != acceptedType.Name)
                countByShortTypeName[acceptedType.Name.TypeName.WithoutContainingType] =
                    fullNameAndCount with { Count = fullNameAndCount.Count + 1 };

            return acceptedType with
            {
                IndexOfAcceptedTypesWithSameShortName =
                countByShortTypeName[acceptedType.Name.TypeName.WithoutContainingType].Count
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

    internal IReadOnlyList<AcceptedTypesForAffectedGenericType> Values { get; }

    internal FullTypeNameCollection FullTypeNames => new(Values.SelectMany(p => p.AcceptedTypes.FullTypeNames.Values));
}