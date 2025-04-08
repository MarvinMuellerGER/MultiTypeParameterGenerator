using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal sealed record ContainingType(TypeKindName? Kind, FullTypeName Name, GenericTypeCollection GenericTypes);