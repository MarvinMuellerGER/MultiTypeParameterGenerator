using MultiTypeParameterGenerator.Common.Factories.Entities;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Factories.Entities;
using MultiTypeParameterGenerator.Generation.Models.Entities;
using NSubstitute;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Factories.Entities;

public static class SourceCodeFileFactoryTest
{
    public sealed class Create
    {
        [Fact]
        public void CreatesSourceCodeFileFromMethodSourceCodesOfType()
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

            var methodToOverload = new MethodToOverload(
                false,
                false,
                new(new("class"), new(new(new("SomeNamespace"), new("SomeClass"))), new()),
                new(),
                new NamedType(new(null, new("void"))),
                new("SomeMethod"),
                new([new(new("T"))]),
                new(),
                new());

            var expectedMethodSourceCodes = new MethodSourceCode(
                false,
                false,
                new(new("class"), new(new(new("SomeNamespace"), new("SomeClass"))), new()), true,
                new("SomeMethod"),
                new("int"),
                new(),
                new(
                    """
                       static void SomeMethod<T>(int x) =>
                          SomeMethod<T>(x);
                    """));

            var methodSourceCodeCollectionFactory = Substitute.For<IMethodSourceCodeFactory>();
            methodSourceCodeCollectionFactory.Create(methodToOverload).Returns(expectedMethodSourceCodes);

            // Act
            var sourceCodeFile = new SourceCodeFileFactory(methodSourceCodeCollectionFactory).Create(methodToOverload);

            // Assert
            sourceCodeFile.FileName.Value.Should().Be("SomeNamespace.SomeClass.SomeMethod.g.cs");
            sourceCodeFile.SourceCode.Value.Should().EndWith(expectedSourceCode);
        }
    }
}