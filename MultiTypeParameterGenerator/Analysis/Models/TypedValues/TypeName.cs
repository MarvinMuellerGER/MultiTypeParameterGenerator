using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Extensions.Collections;
using static MultiTypeParameterGenerator.Analysis.Extensions.StringExtensions;

namespace MultiTypeParameterGenerator.Analysis.Models.TypedValues;

internal abstract record TypeName(string Value)
{
    internal TypeName WithAcceptedTypes(AcceptedTypeCombination acceptedTypeCombination)
    {
        var parameterTypeSplitted =
            Value.ReplaceMultiple(['(', ')', '<', '>', '[', ']', ' ', ','], t => $"|{t}|").Split('|').ToList();

        acceptedTypeCombination.Values.ForEach(acceptedType => parameterTypeSplitted.ReplaceWhere(
            t => acceptedType.AffectedGenericType.Name.Value == t,
            acceptedType.AcceptedType.TypeNameForSourceCode.Value));

        return this with { Value = string.Concat(parameterTypeSplitted) };
    }
}