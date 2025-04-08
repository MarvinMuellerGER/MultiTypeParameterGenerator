using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.Collections;

public class GenericTypeCollectionTest
{
    public class SourceCode
    {
        [Fact]
        public void Should_ContainGenericTypeNamesSeparatedByComma()
        {
            // Arrange
            var collection = new GenericTypeCollection(new(new("T1")), new(new("T2")));

            // Act & Assert
            collection.SourceCode.Value.Should().Be("<T1, T2>");
        }
    }

    public class ExceptAcceptedTypes
    {
        [Fact]
        public void Should_ExcludeAffectedTypeNames()
        {
            // Arrange
            var collection = new GenericTypeCollection(new(new("T1")), new(new("T2")));
            var toExclude = new AcceptedTypeCombination(
                [new(new(new("T1")), false, new(new("bool"), false, false))]
            );

            // Act
            var result = collection.ExceptNoneGenericAcceptedTypes(toExclude);

            // Assert
            result.Values.Should().ContainSingle();
            result.Values[0].Name.Value.Should().Be("T2");
        }
    }

    public class ConstraintsSourceCode
    {
        [Fact]
        public void Should_ReturnEmptySourceCode_When_EmptyCollection()
        {
            // Arrange
            var collection = new GenericTypeCollection();

            // Act & Assert
            collection.ConstraintsSourceCode.Value.Should().Be(string.Empty);
        }

        [Fact]
        public void Should_ReturnEmptySourceCode_When_NoGenericTypesHaveConstraints()
        {
            // Arrange
            var collection = new GenericTypeCollection(
                new GenericType(new("T1")),
                new GenericType(new("T2"))
            );

            // Act & Assert
            collection.ConstraintsSourceCode.Value.Should().Be(string.Empty);
        }

        [Fact]
        public void Should_CombineConstraints_When_SingleGenericTypeHasConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), true, false, false, false, false, false);
            var collection = new GenericTypeCollection(
                new GenericType(new("T1"), constraint),
                new GenericType(new("T2"))
            );

            // Act & Assert
            collection.ConstraintsSourceCode.Value.Should().Be(
                """

                      where T1 : struct
                """);
        }

        [Fact]
        public void Should_CombineConstraints_When_MultipleGenericTypesHaveConstraints()
        {
            // Arrange
            var constraint1 = new TypeConstraint(new(), true, false, false, false, false, false);
            var constraint2 =
                new TypeConstraint(new(new TypeName("IComparable")), false, true, true, false, false, false);

            var collection = new GenericTypeCollection(
                new GenericType(new("T1"), constraint1),
                new GenericType(new("T2"), constraint2)
            );

            // Act & Assert
            collection.ConstraintsSourceCode.Value.Should().Be(
                """

                      where T1 : struct
                      where T2 : IComparable, class?
                """);
        }
    }
}