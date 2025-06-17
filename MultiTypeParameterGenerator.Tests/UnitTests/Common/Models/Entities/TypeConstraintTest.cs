using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.Entities;

public static class TypeConstraintTest
{
    public sealed class SourceCodeProperty
    {
        [Fact]
        public void ReturnsNull_WhenNoConstraints()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, false, false, false, false, false);

            // Act & Assert
            constraint.SourceCode.Should().BeNull();
        }

        [Fact]
        public void ReturnsSourceCode_WhenHasValueTypeConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), true, false, false, false, false, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("struct");
        }

        [Fact]
        public void ReturnsSourceCode_WhenHasReferenceTypeConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, true, false, false, false, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("class");
        }

        [Fact]
        public void ReturnsSourceCode_WhenHasNullableReferenceTypeConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, true, true, false, false, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("class?");
        }

        [Fact]
        public void ReturnsSourceCode_WhenHasNotNullConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, false, false, true, false, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("notnull");
        }

        [Fact]
        public void ReturnsSourceCode_WhenHasUnmanagedTypeConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, false, false, false, true, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("unmanaged");
        }

        [Fact]
        public void ReturnsSourceCode_WhenHasConstructorConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, false, false, false, false, true);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("new()");
        }

        [Fact]
        public void ReturnsSourceCode_WhenHasTypesConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(new("IComparable"), new("IEnumerable")), false, false, false, false,
                false, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("IComparable, IEnumerable");
        }

        [Fact]
        public void CombinesConstraintsInCorrectOrder_WhenMultipleConstraintsExist()
        {
            // Arrange
            var constraint =
                new TypeConstraint(new(new TypeName("IComparable")), true, false, false, true, false, true);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("IComparable, struct, notnull, new()");
        }
    }
}