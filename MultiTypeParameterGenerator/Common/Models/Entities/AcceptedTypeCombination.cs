using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Collections;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal sealed record AcceptedTypeCombination(params IReadOnlyList<AcceptedTypeForAffectedGenericType> Values)
{
    internal GenericTypeCollection AffectedGenericTypesWithoutGenericAcceptedTypes =>
        new(Values.Where(a => a.AcceptedType.UseTypeConstraint is false)
            .SelectToReadonlyList(a => a.AffectedGenericType));

    internal bool AllUseTypeConstraints => Values.All(a => a.AcceptedType.UseTypeConstraint);
}