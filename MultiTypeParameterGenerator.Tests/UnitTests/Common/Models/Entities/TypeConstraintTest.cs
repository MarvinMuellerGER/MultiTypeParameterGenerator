using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.Entities;

public class TypeConstraintTest
{
    public class SourceCodeProperty
    {
        [Fact]
        public void Should_ReturnNull_WhenNoConstraints()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, false, false, false, false, false);

            // Act & Assert
            constraint.SourceCode.Should().BeNull();
        }

        [Fact]
        public void Should_ReturnSourceCode_WhenHasValueTypeConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), true, false, false, false, false, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("struct");
        }

        [Fact]
        public void Should_ReturnSourceCode_WhenHasReferenceTypeConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, true, false, false, false, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("class");
        }

        [Fact]
        public void Should_ReturnSourceCode_WhenHasNullableReferenceTypeConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, true, true, false, false, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("class?");
        }

        [Fact]
        public void Should_ReturnSourceCode_WhenHasNotNullConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, false, false, true, false, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("notnull");
        }

        [Fact]
        public void Should_ReturnSourceCode_WhenHasUnmanagedTypeConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, false, false, false, true, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("unmanaged");
        }

        [Fact]
        public void Should_ReturnSourceCode_WhenHasConstructorConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(), false, false, false, false, false, true);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("new()");
        }

        [Fact]
        public void Should_ReturnSourceCode_WhenHasTypesConstraint()
        {
            // Arrange
            var constraint = new TypeConstraint(new(new("IComparable"), new("IEnumerable")), false, false, false, false,
                false, false);

            // Act & Assert
            constraint.SourceCode.Should().NotBeNull();
            constraint.SourceCode!.Value.Should().Be("IComparable, IEnumerable");
        }

        [Fact]
        public void Should_CombineConstraintsInCorrectOrder_WhenMultipleConstraintsExist()
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