using MultiTypeParameterGenerator.Generation.Models.Collections;
using MultiTypeParameterGenerator.Generation.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Generation.Models.Collections;

public class MethodSourceCodeCollectionTest
{
    private static readonly MethodSourceCode MethodSourceCode = new(
        true,
        new(new("Class"), new(new("SomeNamespace"), new("SomeClass")), new()),
        false,
        new("MethodA"));

    public class AnyMethodToOverloadIsStaticProperty
    {
        [Fact]
        public void ShouldReturn_TrueIfAnyMethodIsStatic()
        {
            // Arrange
            var collection = new MethodSourceCodeCollection(
                MethodSourceCode with { MethodToOverloadIsStatic = true },
                MethodSourceCode with { MethodToOverloadIsStatic = false });

            // Act & Assert
            collection.AnyMethodToOverloadIsStatic.Should().BeTrue();
        }

        [Fact]
        public void ShouldReturn_FalseIfNoMethodIsStatic()
        {
            // Arrange
            var collection = new MethodSourceCodeCollection(
                MethodSourceCode with { MethodToOverloadIsStatic = false },
                MethodSourceCode with { MethodToOverloadIsStatic = false });

            // Act & Assert
            collection.AnyMethodToOverloadIsStatic.Should().BeFalse();
        }
    }

    public class SourceCodeProperty
    {
        [Fact]
        public void ShouldJoinMethodSourceCodesUsingDoubleNewLine()
        {
            // Arrange
            var collection = new MethodSourceCodeCollection(
                MethodSourceCode with { SourceCode = new("MethodA") },
                MethodSourceCode with { SourceCode = new("MethodB") });

            // Act & Assert
            collection.SourceCode.Value.Should().Be(
                """
                MethodA

                MethodB
                """);
        }
    }

    public class GroupedByContainingTypeProperty
    {
        [Fact]
        public void ShouldGroupMethodsByContainingTypeAndKind()
        {
            // Arrange
            var collection = new MethodSourceCodeCollection(
                MethodSourceCode, MethodSourceCode with
                {
                    ContainingType = MethodSourceCode.ContainingType with
                    {
                        Name = new(new("SomeOtherNamespace"), new("SomeOtherClass"))
                    }
                });

            // Act & Assert
            collection.GroupedByContainingType.Values.Should().HaveCount(2);
        }
    }
}