using MultiTypeParameterGenerator.Analysis.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.Entities;

public static class AcceptedTypeTest
{
    public sealed class ShortName
    {
        [Fact]
        public void ReturnsShortNameOfAcceptedTypeName()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, false);

            // Act & Assert
            acceptedType.ShortName.Should().BeEquivalentTo(acceptedType.Name.TypeName);
        }
    }

    public sealed class NameInclNullableAnnotation
    {
        [Fact]
        public void ReturnsFullName_WhenIsNullableIsFalse()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, false);

            // Act & Assert
            acceptedType.TypeNameInclNullableAnnotation.Value.Should().BeEquivalentTo(acceptedType.Name.TypeName.Value);
        }

        [Fact]
        public void ReturnsFullNameFollowedByAQuestionMark_WhenIsNullableIsTrue()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), true, false);

            // Act & Assert
            acceptedType.TypeNameInclNullableAnnotation.Value.Should()
                .BeEquivalentTo($"{acceptedType.Name.TypeName.Value}?");
        }
    }

    public sealed class NameForMethodName
    {
        [Fact]
        public void ReturnsShortName_WhenIndexOfAcceptedTypesWithSameShortNameIsBelowThanOne()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, false);

            // Act & Assert
            acceptedType.NameForMethodName.Value.Should().BeEquivalentTo(acceptedType.ShortName.Value);
        }

        [Fact]
        public void ReturnsShortNameFollowedByIndexOfAcceptedTypesWithSameShortName_WhenItIsAboveThanZero()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, false, 2);

            // Act & Assert
            acceptedType.NameForMethodName.Value.Should().BeEquivalentTo($"{acceptedType.ShortName.Value}_2");
        }
    }

    public sealed class TypeNameForSourceCode
    {
        [Fact]
        public void ReturnsTypeName_WhenUseTypeConstraintIsFalse()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, false);

            // Act & Assert
            acceptedType.TypeNameForSourceCode.Value.Should().BeEquivalentTo(acceptedType.Name.TypeName.Value);
        }

        [Fact]
        public void ReturnsGenericTypeOfShortName_WhenUseTypeConstraintIsTrue()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, true);

            // Act & Assert
            acceptedType.TypeNameForSourceCode.Value.Should().Be($"T{acceptedType.ShortName}");
        }

        [Fact]
        public void ReturnsGenericTypeOfShortNameIncludingNumber_WhenItsNotTheFirstOfThisType()
        {
            // Arrange
            var acceptedType = new AcceptedType(new(new("SomeNamespace"), new("SomeClass")), false, true,
                IndexOfParametersWithSameType: 2);

            // Act & Assert
            acceptedType.TypeNameForSourceCode.Value.Should().BeEquivalentTo($"T{acceptedType.ShortName}_2");
        }
    }
}