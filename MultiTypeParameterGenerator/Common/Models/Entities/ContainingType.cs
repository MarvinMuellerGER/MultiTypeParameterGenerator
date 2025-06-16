using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal sealed record ContainingType(TypeKindName? Kind, FullTypeName Name, GenericTypeCollection GenericTypes)
{
    internal FullTypeName NameInclGenericTypes => new(Name.Namespace, new($"{Name.TypeName}{GenericTypes.SourceCode}"));

    internal TypeName FullNameWithGenericTypesCount =>
        new(Name + (GenericTypes.Values.Count is 0 ? string.Empty : $"`{GenericTypes.Values.Count}"));
}