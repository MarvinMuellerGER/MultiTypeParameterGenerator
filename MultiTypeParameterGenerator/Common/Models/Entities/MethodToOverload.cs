using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Models.Collections;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal sealed record MethodToOverload(
    bool UseFullTypeNames,
    bool GenerateExtensionMethod,
    ContainingType ContainingType,
    AccessModifierNameCollection AccessModifiers,
    FullTypeName ReturnType,
    MethodName Name,
    GenericTypeCollection GenericTypes,
    AcceptedTypesForAffectedGenericTypeCollection AffectedGenericTypes,
    ParameterCollection Parameters)
{
    internal bool MethodToOverloadIsStatic => AccessModifiers.Values.Select(v => v.Value).Contains("static");

    internal FullTypeNameCollection FullTypeNames =>
        Parameters.FullTypeNames.ConcatDistinct(AffectedGenericTypes.FullTypeNames).ConcatDistinct(ReturnType);
}