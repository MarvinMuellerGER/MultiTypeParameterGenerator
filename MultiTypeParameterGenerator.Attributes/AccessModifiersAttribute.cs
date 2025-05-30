namespace MultiTypeParameterGenerator;

public enum AccessModifier
{
    Public,
    Protected,
    Internal,
    ProtectedInternal,
    PrivateProtected
}

[AttributeUsage(AttributeTargets.Method)]
#pragma warning disable CS9113 // Parameter is unread.
public sealed class AccessModifiersAttribute(AccessModifier accessModifiers) : Attribute;
#pragma warning restore CS9113 // Parameter is unread.