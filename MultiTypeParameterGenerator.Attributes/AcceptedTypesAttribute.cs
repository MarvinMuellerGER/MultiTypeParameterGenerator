// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedTypeParameter

namespace MultiTypeParameterGenerator;

/// <summary>
///     An Attribute that defines the accepted types for a generic parameter.
///     This attribute is used by the MultiTypeParameterGenerator to determine which types should be used
///     when generating method overloads.
/// </summary>
/// <param name="asGenericTypes">
///     If true, the types will be treated as generic type parameters;
///     if false, they will be treated as concrete types. (default: false)
/// </param>
/// <param name="additionalTypes">
///     Additional type names that should be included as accepted types,
///     specified as fully qualified type names.
/// </param>
[AttributeUsage(AttributeTargets.GenericParameter)]
#pragma warning disable CS9113 // Parameter is unread.
public abstract class AcceptedTypesAttribute(bool asGenericTypes, params string[] additionalTypes) : Attribute;
#pragma warning restore CS9113 // Parameter is unread.

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<TCollection>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes) where TCollection : struct, IAcceptedTypesCollection
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}

/// <inheritdoc />
public sealed class AcceptedTypesAttribute<
    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(
    bool asGenericTypes,
    params string[] additionalTypes)
    : AcceptedTypesAttribute(asGenericTypes, additionalTypes)
{
    /// <inheritdoc />
    public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
}