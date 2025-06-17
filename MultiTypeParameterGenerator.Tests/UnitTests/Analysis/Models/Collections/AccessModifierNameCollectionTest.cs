using MultiTypeParameterGenerator.Analysis.Models.Collections;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.Collections;

public static class AccessModifierNameCollectionTest
{
    public sealed class IsEmpty
    {
        [Fact]
        public void ReturnsTrue_WhenCollectionIsEmpty()
        {
            // Arrange
            var collection = new AccessModifierNameCollection();

            // Act & Assert
            collection.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void ReturnsFalse_WhenCollectionIsNotEmpty()
        {
            // Arrange
            var collection = new AccessModifierNameCollection([new("public")]);

            // Act & Assert
            collection.IsEmpty.Should().BeFalse();
        }
    }

    public sealed class SourceCodeProperty
    {
        [Fact]
        public void ReturnsCorrectlyJoinedString()
        {
            // Arrange
            var collection = new AccessModifierNameCollection(new("public"), new("internal"));

            // Act & Assert
            collection.SourceCode.Value.Should().Be("public internal");
        }
    }
}