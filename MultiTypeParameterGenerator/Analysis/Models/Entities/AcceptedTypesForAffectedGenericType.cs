using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal readonly record struct AcceptedTypesForAffectedGenericType(
    GenericType AffectedGenericType,
    AcceptedTypeCollection AcceptedTypes);