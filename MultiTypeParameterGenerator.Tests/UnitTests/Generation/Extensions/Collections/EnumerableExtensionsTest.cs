using MultiTypeParameterGenerator.Generation.Extensions.Collections;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Generation.Extensions.Collections;

public static class EnumerableExtensionsTest
{
    public sealed class ToReadonlyList
    {
        [Fact]
        public void ReturnsExactReadonlyList()
        {
            // Arrange
            var original = new[] { 1, 2, 3 };

            // Act
            var result = original.ToReadonlyList();

            // Assert
            result.Should().BeAssignableTo<IReadOnlyList<int>>();
            result.Should().ContainInOrder(1, 2, 3);
        }
    }
}