using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using MultiTypeParameterGenerator.Generation.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Attributes;

internal class AcceptedTypesAttributeDefinitionFactory : IAcceptedTypesAttributeDefinitionFactory
{
    private static readonly Namespace Namespace = new("MultiTypeParameterGenerator");
    private static readonly TypeName TypeName = new("AcceptedTypesAttribute");
    private static readonly FullTypeName FullName = new(Namespace, TypeName);
    private static readonly FileName FileName = new($"{TypeName}.g.cs");

    private static readonly SourceCode SourceCode =
        new($$"""
              namespace {{Namespace}};

              public struct GenericType<T>;

              [AttributeUsage(AttributeTargets.GenericParameter)]
              #pragma warning disable CS9113 // Parameter is unread.
              public abstract class {{TypeName}}(
                 bool AsGenericTypes, params string[] AdditionalTypes) : Attribute;
              #pragma warning restore CS9113 // Parameter is unread.

              public sealed class {{TypeName}}<T1, T2>(
                 bool AsGenericTypes, params string[] AdditionalTypes)
                 : {{TypeName}}(AsGenericTypes, AdditionalTypes)
              {
                  public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
              }

              public sealed class {{TypeName}}<T1, T2, T3>(
                 bool AsGenericTypes, params string[] AdditionalTypes)
                 : {{TypeName}}(AsGenericTypes, AdditionalTypes)
              {
                  public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
              }

              public sealed class {{TypeName}}<T1, T2, T3, T4>(
                 bool AsGenericTypes, params string[] AdditionalTypes)
                 : {{TypeName}}(AsGenericTypes, AdditionalTypes)
              {
                  public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
              }

              public sealed class {{TypeName}}<T1, T2, T3, T4, T5>(
                 bool AsGenericTypes, params string[] AdditionalTypes)
                 : {{TypeName}}(AsGenericTypes, AdditionalTypes)
              {
                  public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
              }

              public sealed class {{TypeName}}<T1, T2, T3, T4, T5, T6>(
                 bool AsGenericTypes, params string[] AdditionalTypes)
                 : {{TypeName}}(AsGenericTypes, AdditionalTypes)
              {
                  public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
              }

              public sealed class {{TypeName}}<T1, T2, T3, T4, T5, T6, T7>(
                 bool AsGenericTypes, params string[] AdditionalTypes)
                 : {{TypeName}}(AsGenericTypes, AdditionalTypes)
              {
                  public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
              }

              public sealed class {{TypeName}}<T1, T2, T3, T4, T5, T6, T7, T8>(
                 bool AsGenericTypes, params string[] AdditionalTypes)
                 : {{TypeName}}(AsGenericTypes, AdditionalTypes)
              {
                  public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
              }

              public sealed class {{TypeName}}<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
                 bool AsGenericTypes, params string[] AdditionalTypes)
                 : {{TypeName}}(AsGenericTypes, AdditionalTypes)
              {
                  public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
              }

              public sealed class {{TypeName}}<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                 bool AsGenericTypes, params string[] AdditionalTypes)
                 : {{TypeName}}(AsGenericTypes, AdditionalTypes)
              {
                  public AcceptedTypesAttribute(params string[] additionalTypes) : this(false, additionalTypes) { }
              }
              """);

    private static readonly SourceCodeFile SourceCodeFile = new(FileName, SourceCode);

    public AttributeDefinition Create() => new(FullName, SourceCodeFile);
}