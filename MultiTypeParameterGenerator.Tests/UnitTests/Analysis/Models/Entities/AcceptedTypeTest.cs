using MultiTypeParameterGenerator.Analysis.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.Entities;

public class AcceptedTypeTest
{
    public class ShortName
    {
        [Fact]
        public void Should_ReturnShortNameOfAcceptedTypeName()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, false);

            // Act & Assert
            acceptedType.ShortName.Should().BeEquivalentTo(acceptedType.Name.TypeName);
        }
    }

    public class NameInclNullableAnnotation
    {
        [Fact]
        public void Should_ReturnFullName_WhenIsNullableIsFalse()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, false);

            // Act & Assert
            acceptedType.TypeNameInclNullableAnnotation.Value.Should().BeEquivalentTo(acceptedType.Name.TypeName.Value);
        }

        [Fact]
        public void Should_ReturnFullNameFollowedByAQuestionMark_WhenIsNullableIsTrue()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), true, false);

            // Act & Assert
            acceptedType.TypeNameInclNullableAnnotation.Value.Should()
                .BeEquivalentTo($"{acceptedType.Name.TypeName.Value}?");
        }
    }

    public class NameForMethodName
    {
        [Fact]
        public void Should_ReturnShortName_WhenIndexOfAcceptedTypesWithSameShortNameIsBelowThanOne()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, false);

            // Act & Assert
            acceptedType.NameForMethodName.Value.Should().BeEquivalentTo(acceptedType.ShortName.Value);
        }

        [Fact]
        public void Should_ReturnShortNameFollowedByIndexOfAcceptedTypesWithSameShortName_WhenItIsAboveThanZero()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, false, 2);

            // Act & Assert
            acceptedType.NameForMethodName.Value.Should().BeEquivalentTo($"{acceptedType.ShortName.Value}_2");
        }
    }

    public class TypeNameForSourceCode
    {
        [Fact]
        public void Should_ReturnTypeName_WhenUseTypeConstraintIsFalse()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, false);

            // Act & Assert
            acceptedType.TypeNameForSourceCode.Value.Should().BeEquivalentTo(acceptedType.Name.TypeName.Value);
        }

        [Fact]
        public void Should_ReturnGenericTypeOfShortName_WhenUseTypeConstraintIsTrue()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, true);

            // Act & Assert
            acceptedType.TypeNameForSourceCode.Value.Should().Be($"T{acceptedType.ShortName}");
        }

        [Fact]
        public void Should_ReturnGenericTypeOfShortNameIncludingNumber_WhenItsNotTheFirstOfThisType()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, true,
                IndexOfParametersWithSameType: 2);

            // Act & Assert
            acceptedType.TypeNameForSourceCode.Value.Should().BeEquivalentTo($"T{acceptedType.ShortName}_2");
        }
    }
}