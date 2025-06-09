using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.Entities;

public class MethodToOverloadTest
{
    private static readonly MethodToOverload MethodToOverload = new(
        false,
        true,
        new(new("Class"), new(new("SomeNamespace"), new("SomeClass")), new()),
        new([new("public")]),
        new(null, new("void")),
        new("SomeMethod"),
        new(),
        new(),
        new([new(new(null, new("int")), new("x"))]));

    public class MethodToOverloadIsStatic
    {
        [Fact]
        public void Should_BeFalse_WhenAccessModifiersDontContainStatic()
        {
            // Act & Assert
            MethodToOverload.MethodToOverloadIsStatic.Should().BeFalse();
        }

        [Fact]
        public void Should_BeTrue_WhenAccessModifiersContainStatic()
        {
            // Arrange
            var method = MethodToOverload with { AccessModifiers = new(new("public"), new("static")) };

            // Act & Assert
            method.MethodToOverloadIsStatic.Should().BeTrue();
        }
    }
}