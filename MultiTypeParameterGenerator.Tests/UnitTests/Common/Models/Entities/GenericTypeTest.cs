using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.Entities;

public class GenericTypeTest
{
    public class CreateFromAcceptedTypeName
    {
        [Fact]
        public void Should_CreateGenericTypeNameWithSameValue()
        {
            // Arrange
            var accepted = new AcceptedType(new(null, new("AcceptedValue")), false, false);

            // Act & Assert
            GenericType.FromAcceptedType(accepted).Name.Value.Should().Be("AcceptedValue");
        }
    }

    public class ConstraintSourceCodeProperty
    {
        [Fact]
        public void Should_ReturnNull_When_ConstraintIsNull()
        {
            // Arrange
            var genericType = new GenericType(new("T"));

            // Act & Assert
            genericType.ConstraintSourceCode.Should().BeNull();
        }

        [Fact]
        public void Should_ReturnNull_When_ConstraintSourceCodeIsNull()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, false, false, false, false, false);
            var genericType = new GenericType(new("T"), constraint);

            // Act & Assert
            genericType.ConstraintSourceCode.Should().BeNull();
        }

        [Fact]
        public void Should_ReturnFormattedConstraint_When_ConstraintSourceCodeIsNotNull()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), true, false, false, false, false, false);
            var genericType = new GenericType(new("T"), constraint);

            // Act & Assert
            genericType.ConstraintSourceCode.Should().NotBeNull();
            genericType.ConstraintSourceCode!.Value.Should().Be(
                """

                      where T : struct
                """);
        }

        [Fact]
        public void Should_IncludeMultipleConstraints_When_ConstraintHasMultipleValues()
        {
            // Arrange
            var constraint =
                new TypeConstraint(new(new TypeName("IComparable")), true, false, false, true, false, true);
            var genericType = new GenericType(new("T"), constraint);

            // Act & Assert
            genericType.ConstraintSourceCode.Should().NotBeNull();
            genericType.ConstraintSourceCode!.Value.Should().Be(
                """

                      where T : IComparable, struct, notnull, new()
                """);
        }
    }
}