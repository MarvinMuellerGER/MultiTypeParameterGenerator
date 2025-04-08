using MultiTypeParameterGenerator.Generation.Extensions.Collections;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Generation.Extensions.Collections;

public class EnumerableExtensionsTest
{
    public class ToReadonlyList
    {
        [Fact]
        public void Should_ReturnExactReadonlyList()
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