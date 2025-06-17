using MultiTypeParameterGenerator.Analysis.Extensions.Collections;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Extensions.Collections;

public static class DictionaryExtensions
{
    public sealed class GetCombinations
    {
        [Fact]
        public void ReturnsAllPossibleCombinationsOnce()
        {
            // Arrange
            var testData = new Dictionary<string, IReadOnlyList<string>>
            {
                { "T1", ["bool", "int"] },
                { "T2", ["string", "long"] },
                { "T3", ["byte", "char"] }
            };
            var expectedResult = new List<IReadOnlyList<(string, string)>>
            {
                new[] { ("T1", "bool"), ("T2", "string"), ("T3", "byte") },
                new[] { ("T1", "bool"), ("T2", "string"), ("T3", "char") },
                new[] { ("T1", "bool"), ("T2", "long"), ("T3", "byte") },
                new[] { ("T1", "bool"), ("T2", "long"), ("T3", "char") },
                new[] { ("T1", "int"), ("T2", "string"), ("T3", "byte") },
                new[] { ("T1", "int"), ("T2", "string"), ("T3", "char") },
                new[] { ("T1", "int"), ("T2", "long"), ("T3", "byte") },
                new[] { ("T1", "int"), ("T2", "long"), ("T3", "char") }
            };

            // Act
            var result = testData.GetCombinations();

            // Assert
            result.Should().HaveCount(8);
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}