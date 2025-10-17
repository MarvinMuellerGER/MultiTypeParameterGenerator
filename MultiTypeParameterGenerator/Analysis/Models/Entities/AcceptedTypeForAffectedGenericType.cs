using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal record AcceptedTypeForAffectedGenericType(
    GenericType AffectedGenericType,
    bool IsNotFirstGenericAcceptedType,
    AcceptedType AcceptedType);