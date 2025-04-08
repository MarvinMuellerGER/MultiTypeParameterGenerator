using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Analysis.Models.TypedValues;

internal sealed record ParameterTypeName(string Value) : TypeName(Value), IEquatable<GenericTypeName>
{
    internal AcceptedTypeName ShortName => new(Value.Split('.').LastOrDefault() ?? Value);

    public bool Equals(GenericTypeName affectedGenericTypeName) => affectedGenericTypeName.Value == Value;

    public override string ToString() => Value;

    public static bool operator ==(ParameterTypeName @this, GenericTypeName affectedGenericTypeName) =>
        @this.Equals(affectedGenericTypeName);

    public static bool operator !=(ParameterTypeName @this, GenericTypeName affectedGenericTypeName) =>
        !@this.Equals(affectedGenericTypeName);
}