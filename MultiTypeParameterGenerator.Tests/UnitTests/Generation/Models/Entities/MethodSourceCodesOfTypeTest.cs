using MultiTypeParameterGenerator.Common.Models.TypedValues;
using MultiTypeParameterGenerator.Generation.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Generation.Models.Entities;

public class MethodSourceCodesOfTypeTest
{
    [Fact]
    public void Should_UseFirstMethodSourceCodeByDefault()
    {
        // Arrange
        var methodSourceCodesOfType1 = new MethodSourceCodesOfType(new(
            new(true, new(new("class"), new(new("SomeNamespace"), new("SomeClass")), new()), false, new("Code1")),
            new(true, new(new("class"), new(new("SomeNamespace"), new("SomeClass")), new()), true, new("Code2"))));

        var methodSourceCodesOfType2 = new MethodSourceCodesOfType(new(
            new(false, new(new("struct"), new(new("SomeNamespace"), new("SomeStruct")),
                new()), false, new("Code1")),
            new(false, new(new("struct"), new(new("SomeNamespace"), new("SomeStruct")),
                new()), true, new("Code2"))));

        // Act & Assert
        methodSourceCodesOfType1.GenerateExtensionClass.Should().BeTrue();
        methodSourceCodesOfType1.ContainingType.Name.ToString().Should().Be("SomeNamespace.SomeClass");
        methodSourceCodesOfType1.ContainingType.Kind.Should().Be(new TypeKindName("class"));

        methodSourceCodesOfType2.GenerateExtensionClass.Should().BeFalse();
        methodSourceCodesOfType2.ContainingType.Name.ToString().Should().Be("SomeNamespace.SomeStruct");
        methodSourceCodesOfType2.ContainingType.Kind.Should().Be(new TypeKindName("struct"));
    }
}