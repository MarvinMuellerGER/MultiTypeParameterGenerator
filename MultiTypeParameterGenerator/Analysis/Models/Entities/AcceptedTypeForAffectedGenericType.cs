using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal readonly record struct AcceptedTypeForAffectedGenericType(
    GenericType AffectedGenericType,
    bool IsNotFirstGenericAcceptedType,
    AcceptedType AcceptedType);