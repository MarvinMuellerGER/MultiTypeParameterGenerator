using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.Entities;

public static class GenericTypeTest
{
    public sealed class CreateFromAcceptedTypeName
    {
        [Fact]
        public void CreatesGenericTypeNameWithSameValue()
        {
            // Arrange
            var accepted = new AcceptedType(new NamedType(new(null, new("AcceptedValue"))), false, false);

            // Act & Assert
            GenericType.FromAcceptedType(accepted, false, true).Name.Value.Should().Be("AcceptedValue");
        }
    }

    public sealed class ConstraintSourceCodeProperty
    {
        [Fact]
        public void ReturnsNull_When_ConstraintIsNull()
        {
            // Arrange
            var genericType = new GenericType(new("T"));

            // Act & Assert
            genericType.ConstraintSourceCode.Should().BeNull();
        }

        [Fact]
        public void ReturnsNull_When_ConstraintSourceCodeIsNull()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, false, false, false, false, false);
            var genericType = new GenericType(new("T"), constraint);

            // Act & Assert
            genericType.ConstraintSourceCode.Should().BeNull();
        }

        [Fact]
        public void ReturnsFormattedConstraint_When_ConstraintSourceCodeIsNotNull()
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
        public void IncludesMultipleConstraints_When_ConstraintHasMultipleValues()
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