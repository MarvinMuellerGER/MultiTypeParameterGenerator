using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.Entities;

public static class AcceptedTypeTest
{
    public sealed class ShortName
    {
        [Fact]
        public void ReturnsShortNameOfAcceptedTypeName()
        {
            // Arrange
            var acceptedType = new AcceptedType(new NamedType(new(new("SomeNamespace"), new("SomeClass"))), false);

            // Act & Assert
            acceptedType.ShortName.Should().BeEquivalentTo(acceptedType.Type.FullTypeNames.Values[0].TypeName);
        }
    }

    public sealed class NameInclNullableAnnotation
    {
        [Fact]
        public void ReturnsFullName_WhenIsNullableIsFalse()
        {
            // Arrange
            var acceptedType = new AcceptedType(new NamedType(new(new("SomeNamespace"), new("SomeClass"))), false);

            // Act & Assert
            acceptedType.TypeNameInclNullableAnnotation.Value.Should()
                .BeEquivalentTo(acceptedType.Type.FullTypeNames.Values[0].TypeName.Value);
        }

        [Fact]
        public void ReturnsFullNameFollowedByAQuestionMark_WhenIsNullableIsTrue()
        {
            // Arrange
            var acceptedType =
                new AcceptedType(new NamedType(new(new("SomeNamespace"), new("SomeClass")), null, new(), true), true);

            // Act & Assert
            acceptedType.TypeNameInclNullableAnnotation.Value.Should()
                .BeEquivalentTo($"{acceptedType.Type.FullTypeNames.Values[0].TypeName.Value}?");
        }
    }

    public sealed class TypeNameForSourceCode
    {
        [Fact]
        public void ReturnsTypeName_WhenUseTypeConstraintIsFalse()
        {
            // Arrange
            var acceptedType = new AcceptedType(new NamedType(new(new("SomeNamespace"), new("SomeClass"))), false);

            // Act & Assert
            acceptedType.TypeNameForSourceCode.Value.Should()
                .BeEquivalentTo(acceptedType.Type.FullTypeNames.Values[0].TypeName.Value);
        }

        [Fact]
        public void ReturnsGenericTypeOfShortName_WhenUseTypeConstraintIsTrue()
        {
            // Arrange
            var acceptedType = new AcceptedType(new NamedType(new(new("SomeNamespace"), new("SomeClass"))), true);

            // Act & Assert
            acceptedType.TypeNameForSourceCode.Value.Should().Be($"T{acceptedType.ShortName}");
        }

        [Fact]
        public void ReturnsGenericTypeOfShortNameIncludingNumber_WhenItsNotTheFirstOfThisType()
        {
            // Arrange
            var acceptedType = new AcceptedType(new NamedType(new(new("SomeNamespace"), new("SomeClass"))), true, 2);

            // Act & Assert
            acceptedType.TypeNameForSourceCode.Value.Should().BeEquivalentTo($"T{acceptedType.ShortName}_2");
        }
    }
}