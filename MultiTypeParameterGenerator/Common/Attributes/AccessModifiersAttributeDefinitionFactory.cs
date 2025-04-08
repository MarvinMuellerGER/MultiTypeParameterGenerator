using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using MultiTypeParameterGenerator.Generation.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Attributes;

internal class AccessModifiersAttributeDefinitionFactory : IAccessModifiersAttributeDefinitionFactory
{
    private static readonly Namespace Namespace = new("MultiTypeParameterGenerator");
    private static readonly TypeName TypeName = new("AccessModifiersAttribute");
    private static readonly FullTypeName FullName = new(Namespace, TypeName);
    private static readonly FileName FileName = new($"{TypeName}.g.cs");

    private static readonly SourceCode SourceCode =
        new($"""
             namespace {Namespace};

             [AttributeUsage(AttributeTargets.Method)]
             #pragma warning disable CS9113 // Parameter is unread.
             public sealed class {TypeName}(AccessModifier accessModifiers) : Attribute;
             #pragma warning restore CS9113 // Parameter is unread.
             """);

    private static readonly SourceCodeFile SourceCodeFile = new(FileName, SourceCode);

    public AttributeDefinition Create() => new(FullName, SourceCodeFile);
}