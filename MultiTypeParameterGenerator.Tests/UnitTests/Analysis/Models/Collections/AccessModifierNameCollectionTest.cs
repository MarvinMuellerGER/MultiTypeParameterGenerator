using MultiTypeParameterGenerator.Analysis.Models.Collections;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.Collections;

public class AccessModifierNameCollectionTest
{
    public class IsEmpty
    {
        [Fact]
        public void Should_ReturnTrue_WhenCollectionIsEmpty()
        {
            // Arrange
            var collection = new AccessModifierNameCollection();

            // Act & Assert
            collection.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void Should_ReturnFalse_WhenCollectionIsNotEmpty()
        {
            // Arrange
            var collection = new AccessModifierNameCollection([new("public")]);

            // Act & Assert
            collection.IsEmpty.Should().BeFalse();
        }
    }

    public class SourceCodeProperty
    {
        [Fact]
        public void Should_ReturnCorrectlyJoinedString()
        {
            // Arrange
            var collection = new AccessModifierNameCollection(new("public"), new("internal"));

            // Act & Assert
            collection.SourceCode.Value.Should().Be("public internal");
        }
    }
}