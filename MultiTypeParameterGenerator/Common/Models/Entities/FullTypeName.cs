using MultiTypeParameterGenerator.Common.Extensions;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal sealed record FullTypeName(Namespace? Namespace, TypeName TypeName, TypeAlias? Alias = null)
{
    internal string Value => Namespace is null ? TypeName.Value : $"{Namespace}.{TypeName}";

    public override string ToString() => Value;

    internal FullTypeName WithAcceptedTypes(AcceptedTypeCombination acceptedTypeCombination) =>
        this with { TypeName = new(ReplaceAffectedByAcceptedTypes(TypeName.Value, acceptedTypeCombination)) };

    private static string ReplaceAffectedByAcceptedTypes(
        string typeOrNamespaceName, AcceptedTypeCombination acceptedTypeCombination)
    {
        var parameterTypeSplitted = typeOrNamespaceName
            .ReplaceMultiple(['(', ')', '<', '>', '[', ']', ' ', ','], t => $"|{t}|").Split('|').ToList();

        acceptedTypeCombination.Values.ForEach(acceptedType => parameterTypeSplitted.ReplaceWhere(
            t => acceptedType.AffectedGenericType.Name.Value == t,
            acceptedType.AcceptedType.TypeNameForSourceCode.Value));

        return string.Concat(parameterTypeSplitted);
    }
}