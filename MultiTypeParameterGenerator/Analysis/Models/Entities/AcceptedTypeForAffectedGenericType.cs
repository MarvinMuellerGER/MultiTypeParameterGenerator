using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal record AcceptedTypeForAffectedGenericType(
    GenericType AffectedGenericType,
    AcceptedType AcceptedType,
    bool IsNotFirstGenericAcceptedType);