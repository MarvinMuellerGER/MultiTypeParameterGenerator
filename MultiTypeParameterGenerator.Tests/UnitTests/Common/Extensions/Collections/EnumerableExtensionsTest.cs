using System.Collections.Immutable;
using MultiTypeParameterGenerator.Common.Extensions.Collections;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Extensions.Collections;

public class EnumerableExtensionsTest
{
    public class WhereNotNull
    {
        [Fact]
        public void Should_ReturnAllItemsThatAreNotNull()
        {
            // Arrange
            int?[] data = [null, 2, null, null, 5];

            // Act
            var result = data.WhereNotNull();

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainInOrder(2, 5);
        }
    }

    public class WhereToReadonly
    {
        [Fact]
        public void Should_FilterItemsCorrectly()
        {
            // Arrange
            var data = new[] { 1, 2, 3, 4, 5 };

            // Act
            var result = data.WhereToReadonlyList(x => x > 3);

            // Assert
            result.Should().HaveCount(2);
            result.Should().ContainInOrder(4, 5);
        }

        [Fact]
        public void Should_ReturnReadOnlyList()
        {
            // Arrange
            var data = Enumerable.Empty<bool>();

            // Act
            var result = data.WhereToReadonlyList(_ => true);

            // Assert
            result.Should().BeAssignableTo<IReadOnlyList<bool>>();
        }
    }

    public class SelectToReadonlyList
    {
        [Fact]
        public void Should_SelectAndReturnReadonlyList()
        {
            // Arrange
            var original = new[] { 1, 2, 3 };

            // Act
            var result = original.SelectToReadonlyList(x => x * 2);

            // Assert
            result.Should().BeAssignableTo<IReadOnlyList<int>>();
            result.Should().ContainInOrder(2, 4, 6);
        }
    }

    public class SelectManyToReadonlyList
    {
        [Fact]
        public void Should_SelectAndReturnReadonlyList()
        {
            // Arrange
            var original = new[] { 1, 2, 3 };

            // Act
            var result = original.SelectManyToReadonlyList(x => new[] { x * 2 });

            // Assert
            result.Should().BeAssignableTo<IReadOnlyList<int>>();
            result.Should().ContainInOrder(2, 4, 6);
        }
    }

    public class SelectToImmutableHashSet
    {
        [Fact]
        public void Should_SelectAndReturnImmutableHashSe()
        {
            // Arrange
            var original = new[] { "a", "b", "b" };

            // Act
            var result = original.SelectToImmutableHashSet(x => x);

            // Assert
            result.Should().BeAssignableTo<ImmutableHashSet<string>>();
            result.Should().HaveCount(2);
        }
    }

    public class Join
    {
        [Fact]
        public void Should_CombineToStringOutputsOfElementsIntoString()
        {
            // Arrange
            var values = new[] { "one", "two", "three" };

            // Act & Assert
            values.Join(";").Should().Be("one;two;three");
        }

        [Fact]
        public void Should_UseCommaFollowedByWhitespaceAsDefaultSeparator()
        {
            // Arrange
            var values = new[] { "one", "two", "three" };

            // Act & Assert
            values.Join().Should().Be("one, two, three");
        }
    }
}