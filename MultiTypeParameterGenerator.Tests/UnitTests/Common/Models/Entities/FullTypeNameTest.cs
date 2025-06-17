using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.Entities;

public static class FullTypeNameTest
{
    public sealed class ValueProperty
    {
        [Fact]
        public void CombinesNamespaceAndTypeName()
        {
            // Arrange
            var fullTypeName = new FullTypeName(new("SomeNamespace"), new("SomeClass"));

            // Act & Assert
            fullTypeName.Value.Should().Be("SomeNamespace.SomeClass");
        }
    }
}