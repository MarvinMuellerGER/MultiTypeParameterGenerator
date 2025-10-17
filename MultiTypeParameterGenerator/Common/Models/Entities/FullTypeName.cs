using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Entities;

internal sealed record FullTypeName(Namespace? Namespace, TypeName TypeName, TypeAlias? Alias = null)
{
    internal string Value => Namespace is null ? TypeName.Value : $"{Namespace}.{TypeName}";

    public override string ToString() => Value;

    internal FullTypeName WithAcceptedTypes(AcceptedTypeCombination acceptedTypeCombination) =>
        this with { TypeName = new(ReplaceByAcceptedType(TypeName.Value, acceptedTypeCombination)) };

    private static string ReplaceByAcceptedType(string typeName, AcceptedTypeCombination acceptedTypeCombination) =>
        acceptedTypeCombination.Values
            .FirstOrDefault(acceptedType => acceptedType.AffectedGenericType.Name.Value == typeName)
            ?.AcceptedType.TypeNameForSourceCode.Value ?? typeName;
}