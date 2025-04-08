using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.Entities;

public class FullTypeNameTest
{
    public class ValueProperty
    {
        [Fact]
        public void Should_CombineNamespaceAndTypeName()
        {
            // Arrange
            var fullTypeName = new FullTypeName(new("SomeNamespace"), new("SomeClass"));

            // Act & Assert
            fullTypeName.Value.Should().Be("SomeNamespace.SomeClass");
        }
    }
}