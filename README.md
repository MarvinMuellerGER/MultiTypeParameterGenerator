# MultiTypeParameterGenerator

[![NuGet](https://img.shields.io/nuget/v/MultiTypeParameterGenerator)](https://www.nuget.org/packages/MultiTypeParameterGenerator/)
[![License](https://img.shields.io/badge/License-Apache2.0-green.svg)](LICENSE)
[![GitHub Build](https://github.com/MarvinMuellerGER/MultiTypeParameterGenerator/actions/workflows/dotnet.yml/badge.svg)](https://github.com/MarvinMuellerGER/MultiTypeParameterGenerator/actions/workflows/dotnet.yml)

A C# source generator that creates method variants with multiple types for a single parameter, reducing boilerplate code
and enabling type-safe APIs with broad type support.

## Overview

MultiTypeParameterGenerator helps you avoid writing repetitive code when you need methods that accept multiple types for
the same parameter. Instead of manually creating overloads for each type, this generator automatically creates them for
you based on simple annotations.

## Installation

Install the NuGet package in your project:

``` bash
dotnet add package MultiTypeParameterGenerator
```

Or add it directly to your `.csproj` file:

``` xml
<ItemGroup>
    <PackageReference Include="MultiTypeParameterGenerator" />
</ItemGroup>
```

## Usage

_**Note:**_ _The following examples are simplified for the sake of brevity. For a more real-world example, see the
[example use case](#example-use-case-real-world) at the end of this document._

### Basic Example

Add an `[AccessModifiers]` attribute to your method and an `[AcceptedTypes]` attribute to your generic type parameter:

``` csharp
using MultiTypeParameterGenerator;

public partial class Calculator
{
    [AccessModifier(Public)] private T Add<[AcceptedTypes<int, double>] T>(int first, T second)
    {
        // Implementation
        var result = first + Convert.ToDouble(second);
        return (T)Convert.ChangeType(result, typeof(T));
    }
}
```

The generator will create method overloads for each specified type:

``` csharp
// Generated code
partial class Calculator
{
    public int Add(int first, int value) => Add<int>(first, second);
    
    public double Add(int first, double value) => Add<double>(first, second);
}
```

### Multiple Parameters

You can specify accepted types for multiple parameters to generate overloads for different combinations:

``` csharp
public void Plot<[AcceptedTypes<int, double>] T1, [AcceptedTypes<int, double>] T2>(T1 x, T2 y)
{
    // Implementation
}

// Generates:
// public void Plot(int x, int y);
// public void Plot(int x, double y);
// public void Plot(double x, int y);
// public void Plot(double x, double y);
```

### Optional Parameters

If you assign a default value to a parameter, it will be contained in the generated overloads too:
``` csharp
public void Plot<[AcceptedTypes<int, double>] T>(T x = default, int y = 0)
{
    // Implementation
}

// Generates:
// public void Plot(int x = default, int y = 0);
// public void Plot(double x = default, int y = 0);
```

### XML documentation comments

If you add summary comments they will not get lost:

``` csharp
/// <summary>
///     Calculates sum of two values
/// </summary>
/// <param name="first">First summand</param>
/// <param name="second">Second summand</param>
/// <typeparam name="T1">Type of first summand</typeparam>
/// <typeparam name="T2">Type of second summand</typeparam>
/// <returns>sum of both values</returns>
private T Add<[AcceptedTypes<int, double>] T>(int first, T second)
{
    // Implementation
    var result = first + Convert.ToDouble(second);
    return (T)Convert.ChangeType(result, typeof(T));
}
```

The generated code will contain inheritdoc comments:

``` csharp
// Generated code
partial class Calculator
{
    /// <inheritdoc cref="Add{T}(int, T)" />
    public int Add(int first, int value) => Add<int>(first, second);

    /// <inheritdoc cref="Add{T}(int, T)" />
    public double Add(int first, double value) => Add<double>(first, second);
}
```

### Advanced Usage

#### Custom Type Names

You can also provide additional type names as strings:

``` csharp
public void Process<[AcceptedTypes<int, double>("System.DateTime", "System.DateOnly")] T>(T value)
{
    // Implementation
}

// Generates overloads for int, double, System.DateTime and System.DateOnly
```

#### Generic Types

Generate methods with generic type parameters:

``` csharp
public void Handle<[AcceptedTypes<GenericType<Foo>, string, FooBar>] T>(T data)
{
    // Implementation
}

// Generates:
// public void Handle<TFoo>(TFoo data) where TFoo : Foo;
// public void Handle(string data);
// public void Handle(FooBar data);
```

You can also define that generic type parameters should be used for every accepted reference type:

``` csharp
public void Handle<[AcceptedTypes<Foo, string, FooBar>(true)] T>(T data)
{
    // Implementation
}

// Generates:
// public void Handle<TFoo>(TFoo data) where TFoo : Foo;
// public void Handle(string data);
// public void Handle<TFooBar>(TFooBar data) where TFooBar : FooBar;
```

#### Shared Accepted Type Collections

If you need the same accepted types for multiple methods, you do not need to specify them multiple times. Simply use an
AcceptedTypesCollection and give it a (global) type alias:

``` csharp
using AcceptedTypesOfCalculator = AcceptedTypesCollection<int, double>

public partial class Calculator
{
    public T Add<[AcceptedTypes<AcceptedTypesOfCalculator>] T>(int first, T second)
    {
        // Implementation
        var result = first + Convert.ToDouble(second);
        return (T)Convert.ChangeType(result, typeof(T));
    }

    public T Subtract<[AcceptedTypes<AcceptedTypesOfCalculator>] T>(int first, T second)
    {
        // Implementation
        var result = first - Convert.ToDouble(second);
        return (T)Convert.ChangeType(result, typeof(T));
    }
}

// Generates:
// public int Add(int first, int value);
// public double Add(int first, double value);
// public int Subtract(int first, int value);
// public double Subtract(int first, double value);
```

## Attributes Reference

### AcceptedTypesAttribute

The main attribute to specify which types are valid for a generic type parameter. Available in various generic versions
to support multiple type parameters.
Basic syntax:

``` csharp
[AcceptedTypes<T1, T2, ...>]
```

Parameters:

- Generic type parameters: Types to generate overloads for
- (optional) `asGenericTypes`: Whether all provided reference types should be treated as generic type arguments
- (optional) `additionalTypes`: Additional type names as strings

Example:

``` csharp
[AcceptedTypes<int, double>(asGenericTypes: false, additionalTypes: "System.DateTime")]
```

### AccessModifierAttribute

The attribute to specify which access modifier that the generated method overloads will have.
Basic syntax:

``` csharp
[AccessModifier(accessModifier: MultiTypeParameterGenerator.AccessModifier)]
```

Parameters:

- accessModifiers: Access modifiers that the generate overloads will have
  ``` csharp
  public enum AccessModifier
  {
      Public,
      Protected,
      Internal,
      ProtectedInternal,
      PrivateProtected
  }
  ```

Example:

``` csharp
[AccessModifier(ProtectedInternal)] private void Foo<[AcceptedTypes<int, double>] T>(T value);
```

## Example Use Case (Real-World)

Finally, a real-world scenario as the examples above are very simple and sterile...

### [Apache Kafka](https://kafka.apache.org) Producer

Let's say you want to create a wrapper around
the [Confluent.Kafka.IProducer<TKey, TValue>](https://docs.confluent.io/platform/current/clients/confluent-kafka-dotnet/_site/api/Confluent.Kafka.IProducer-2.html):

``` csharp
namespace Kafka;

public sealed interface IKafkaProducer
{
    /// <summary>
    /// Send a message to the specified topic.
    /// </summary>
    /// <param name="topic">The topic to send the Kafka message to</param>
    /// <param name="key">The key of the Kafka message</param>
    /// <param name="value">The value of the Kafka message. If the value is 'null', then a Kafka tombstone message will be produced.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <typeparam name="TKey">Key type of the message</typeparam>
    /// <typeparam name="TValue">Value type of the message</typeparam>
    Task ProduceAsync<TKey, TValue>(string topic, TKey key, TValue value, CancellationToken cancellationToken = default);
}

internal sealed class KafkaProducer(IServiceProvider serviceProvider) : IKafkaProducer
{
    public async Task ProduceAsync<TKey, TValue>(string topic, TKey key, TValue value, CancellationToken cancellationToken)
    {
        var producer = serviceProvider.GetRequiredService<IProducer<TKey, TValue>>();
        var message = new Message<TKey, TValue> { Key = key, Value = value };

        return await producer.ProduceAsync(topic, message, cancellationToken);
    }
}
```

With the implementation above you accept any type for the key and value. But that doesn't mean that it will work with
each.
If you provide key or value in any not following mentioned type you will get this exception at runtime:
`System.InvalidOperationException: AvroSerializer only accepts type parameters of int, bool, double, string, float, long, byte[], instances of ISpecificRecord and subclasses of SpecificFixed.`

To avoid this and make it typesafe at compile time, you can use the MultiTypeParameterGenerator to generate overloads
for all supported types:

``` csharp
namespace Kafka;

using MultiTypeParameterGenerator;
using KafkaAcceptedTypes = AcceptedTypesCollection<int, bool, double, string, float, long, byte[], ISpecificRecord, GenericType<SpecificFixed>>;

[AccessModifiers(Public)]
public partial sealed interface IKafkaProducer
{
    /// <summary>
    /// Send a message to the specified topic.
    /// </summary>
    /// <param name="topic">The topic to send the Kafka message to</param>
    /// <param name="key">The key of the Kafka message</param>
    /// <param name="value">The value of the Kafka message. If the value is 'null', then a Kafka tombstone message will be produced.</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <typeparam name="TKey">Key type of the message</typeparam>
    /// <typeparam name="TValue">Value type of the message</typeparam>
    protected Task ProduceAsync<[AcceptedTypes<KafkaAcceptedTypes>] TKey, [AcceptedTypes<KafkaAcceptedTypes>] TValue>(string topic, TKey key, TValue value, CancellationToken cancellationToken = default);
}

internal sealed class KafkaProducer(IServiceProvider serviceProvider) : IKafkaProducer
{
    protected async Task ProduceAsync<TKey, TValue>(string topic, TKey key, TValue value, CancellationToken cancellationToken)
    {
        var producer = serviceProvider.GetRequiredService<IProducer<TKey, TValue>>();
        var message = new Message<TKey, TValue> { Key = key, Value = value };

        return await producer.ProduceAsync(topic, message, cancellationToken);
    }
}
```

This will generate many overloads for the `ProduceAsync` method to accept all but only the specified types:

``` csharp
partial interface IKafkaProducer
{
    /// <inheritdoc cref="ProduceAsync{TKey, TValue}(string, TKey, TValue, CancellationToken)" />
    public Task ProduceAsync(string topic, int key, int value, CancellationToken cancellationToken = default)
        => ProduceAsync<int, int>(topic, key, value, cancellationToken);

    /// <inheritdoc cref="ProduceAsync{TKey, TValue}(string, TKey, TValue, CancellationToken)" />
    public Task ProduceAsync(string topic, int key, bool value, CancellationToken cancellationToken = default)
        => ProduceAsync<int, bool>(topic, key, value, cancellationToken);

    ...

    /// <inheritdoc cref="ProduceAsync{TKey, TValue}(string, TKey, TValue, CancellationToken)" />
    public Task ProduceAsync<TSpecificFixed>(string topic, int TSpecificFixed, float value, CancellationToken cancellationToken = default)
        where TSpecificFixed : SpecificFixed
        => ProduceAsync<int, TSpecificFixed>(topic, key, value, cancellationToken);
}
```

Now your `ProduceAsync` method can only be called with the supported types for key and value, making your API type-safe
and preventing runtime errors! 🥳
And for all of that you didn't have to write loads of boiler code (81 method overloads to be exact)! 😎

## Performance Considerations

- The generated code is created at compile-time, so there's no runtime performance impact
- Generated methods use direct type handling rather than boxing/unboxing
- Code size may increase with many overloads, but execution efficiency is improved

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the Apache License 2.0 - see the LICENSE file for details.
