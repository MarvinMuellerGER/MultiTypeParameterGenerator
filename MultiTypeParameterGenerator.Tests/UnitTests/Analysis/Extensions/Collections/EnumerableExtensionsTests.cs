using MultiTypeParameterGenerator.Analysis.Extensions.Collections;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Extensions.Collections;

public static class EnumerableExtensionsTests
{
    public sealed class GetCombinations
    {
        [Fact]
        public void ReturnsAllCombinations()
        {
            // Arrange
            var lists = new List<IReadOnlyList<int>>
            {
                new List<int> { 1, 2 },
                new List<int> { 3, 4 }
            };

            // Act
            var result = lists.GetCombinations();

            // Assert
            result.Should().HaveCount(4);
            result.Should().BeEquivalentTo(
                new List<IReadOnlyList<int>>
                {
                    new List<int> { 1, 3 },
                    new List<int> { 1, 4 },
                    new List<int> { 2, 3 },
                    new List<int> { 2, 4 }
                });
        }
    }
}