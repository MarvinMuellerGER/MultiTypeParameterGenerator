// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedTypeParameter

namespace MultiTypeParameterGenerator;

public struct GenericType<T>;

[AttributeUsage(AttributeTargets.GenericParameter)]
#pragma warning disable CS9113 // Parameter is unread.
public abstract class AcceptedTypesAttribute(
   bool asGenericTypes, params string[] additionalTypes) : Attribute;
#pragma warning restore CS9113 // Parameter is unread.

public sealed class AcceptedTypesAttribute<T1, T2>(
   bool asGenericTypes, params string[] additionalTypes)
   : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

public sealed class AcceptedTypesAttribute<T1, T2, T3>(
   bool asGenericTypes, params string[] additionalTypes)
   : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

public sealed class AcceptedTypesAttribute<T1, T2, T3, T4>(
   bool asGenericTypes, params string[] additionalTypes)
   : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5>(
   bool asGenericTypes, params string[] additionalTypes)
   : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6>(
   bool asGenericTypes, params string[] additionalTypes)
   : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7>(
   bool asGenericTypes, params string[] additionalTypes)
   : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8>(
   bool asGenericTypes, params string[] additionalTypes)
   : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
   bool asGenericTypes, params string[] additionalTypes)
   : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
   bool asGenericTypes, params string[] additionalTypes)
   : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}