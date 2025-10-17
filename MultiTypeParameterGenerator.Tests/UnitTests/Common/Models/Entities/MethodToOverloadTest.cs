using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.Entities;

public static class MethodToOverloadTest
{
    private static readonly MethodToOverload MethodToOverload = new(
        false,
        true,
        new(new("Class"), new(new(new("SomeNamespace"), new("SomeClass"))), new()),
        new([new("public")]),
        new NamedType(new(null, new("void"))),
        new("SomeMethod"),
        new(),
        new(),
        new([new(new NamedType(new(null, new("int"))), new("x"))]));

    public sealed class MethodToOverloadIsStatic
    {
        [Fact]
        public void IsFalse_WhenAccessModifiersDontContainStatic()
        {
            // Act & Assert
            MethodToOverload.MethodToOverloadIsStatic.Should().BeFalse();
        }

        [Fact]
        public void IsTrue_WhenAccessModifiersContainStatic()
        {
            // Arrange
            var method = MethodToOverload with { AccessModifiers = new(new("public"), new("static")) };

            // Act & Assert
            method.MethodToOverloadIsStatic.Should().BeTrue();
        }
    }
}