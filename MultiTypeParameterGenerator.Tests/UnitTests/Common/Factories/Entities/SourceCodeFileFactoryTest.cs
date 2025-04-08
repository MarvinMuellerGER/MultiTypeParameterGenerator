using MultiTypeParameterGenerator.Common.Factories.Entities;
using MultiTypeParameterGenerator.Generation.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Factories.Entities;

public class SourceCodeFileFactoryTest
{
    public class Create
    {
        [Fact]
        public void Should_CreateSourceCodeFileFromMethodSourceCodesOfType()
        {
            // Arrange
            const string expectedSourceCode =
                """
                namespace SomeNamespace;

                partial class SomeClass
                {
                   static void SomeMethod<T>(int x) =>
                      SomeMethod<T>(x);
                }
                """;

            var methodSourceCodesOfType = new MethodSourceCodesOfType(new([
                new(
                    false,
                    new(new("class"), new(new("SomeNamespace"), new("SomeClass")), new()), true,
                    new(
                        """
                           static void SomeMethod<T>(int x) =>
                              SomeMethod<T>(x);
                        """))
            ]));

            // Act
            var sourceCodeFile = new SourceCodeFileFactory().Create(methodSourceCodesOfType);

            // Assert
            sourceCodeFile.FileName.Value.Should().Be("SomeNamespace.SomeClass.g.cs");
            sourceCodeFile.SourceCode.Value.Should().EndWith(expectedSourceCode);
        }
    }
}