# MultiTypeParameterGenerator
A C# source generator that creates method variants with multiple types for a single parameter, reducing boilerplate code and enabling type-safe APIs with broad type support.
## Overview
MultiTypeParameterGenerator helps you avoid writing repetitive code when you need methods that accept multiple types for the same parameter. Instead of manually creating overloads for each type, this generator automatically creates them for you based on simple annotations.
## Installation
Install the NuGet package in your project:
``` bash
dotnet add package MultiTypeParameterGenerator
```
Or add it directly to your `.csproj` file:
``` xml
<ItemGroup>
    <PackageReference Include="MultiTypeParameterGenerator" Version="1.0.0" />
</ItemGroup>
```
## Usage
### Basic Example
Add an `[AccessModifiers]` to your method and an `[AcceptedTypes]` attribute to your generic type parameter:
``` csharp
using MultiTypeParameterGenerator;

public partial class Calculator
{
    [AccessModifiers(Public)] private T2 Add<T1, [AcceptedTypes<int, double>] T2>(T1 first, T2 second)
    {
        // Implementation
        return second;
    }
}
```
The generator will create method overloads for each specified type:
``` csharp
// Generated code
partial class Calculator
{
    public int Add<T1>(T1 first, int value) => Add<T>(first, second);
    
    public double Add<T1>(T1 first, double value) => Add<T>(first, second);
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
## Attributes Reference
### AcceptedTypesAttribute
The main attribute to specify which types are valid for a generic type parameter. Available in various generic versions to support multiple type parameters.
Basic syntax:
``` csharp
[AcceptedTypes<T1, T2, ...>]
```
Parameters:
- Generic type parameters: Types to generate overloads for
- (optional) `AsGenericTypes`: Whether all provided reference types should be treated as generic type arguments
- (optional) `AdditionalTypes`: Additional type names as strings

Example:
``` csharp
[AcceptedTypes<int, double>(AsGenericTypes: false, AdditionalTypes: "System.DateTime")]
```
### AccessModifiersAttribute
The attribute to specify which access modifiers that the generate overloads will have.
Basic syntax:
``` csharp
[AccessModifiers(AccessModifiers: MultiTypeParameterGenerator.Public)]
```
Parameters:
- AccessModifiers: Access modifiers that the generate overloads will have
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
[AccessModifiers(ProtectedInternal)] private void Foo<[AcceptedTypes<int, double>] T>(T value);
```
## Example Use Cases
### Numerical Operations
``` csharp
public class MathUtility
{
    public T Square<[AcceptedTypes<int, double, decimal>] T>(T value)
    {
        // Implementation
        return value * value;
    }
}
```
### String Handling
``` csharp
public class StringProcessor
{
    public string Normalize<[AcceptedTypes<string, char[]>] T>(T input)
    {
        // Implementation
        return input.ToString();
    }
}
```
## Performance Considerations
- The generated code is created at compile-time, so there's no runtime performance impact
- Generated methods use direct type handling rather than boxing/unboxing
- Code size may increase with many overloads, but execution efficiency is improved

## Contributing
Contributions are welcome! Please feel free to submit a Pull Request.
## License
This project is licensed under the Apache License 2.0 - see the LICENSE file for details.
