using MultiTypeParameterGenerator.Common.Factories.Entities;
using MultiTypeParameterGenerator.Generation.Factories.Collections;
using MultiTypeParameterGenerator.Generation.Models.Collections;
using NSubstitute;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Generation.Factories.Collections;

public class SourceCodeFileCollectionFactoryTest
{
    public class Create
    {
        [Fact]
        public void Should_CreateSourceCodeFileCollectionFromMethodToOverloadCollection()
        {
            // Arrange
            var methodsToOverload = new MethodToOverloadCollection(
                new(
                    false,
                    new(new("class"), new(new("SomeNamespace"), new("SomeClass")), new()),
                    new(),
                    new(new("void")),
                    new("SomeMethod"),
                    new([new(new("T"))]),
                    new(),
                    new()),
                new(
                    false,
                    new(new("class"), new(new("SomeNamespace"), new("SomeOtherClass")), new()),
                    new(),
                    new(new("int")),
                    new("SomeOtherMethod"),
                    new([new(new("U"))]),
                    new(),
                    new([new(new("int"), new("x"))])));

            var expectedMethodSourceCodes = new MethodSourceCodeCollection(
                new(false,
                    new(new("class"), new(new("SomeNamespace"), new("SomeClass")), new()),
                    false,
                    new("""
                           void SomeMethod<T>() =>
                              SomeMethod<T>();
                        """)),
                new(false,
                    new(new("class"), new(new("SomeNamespace"), new("SomeOtherClass")), new()),
                    false,
                    new("""
                           int SomeOtherMethod<U>(int x) =>
                              SomeOtherMethod<U>(x);
                        """)));

            var methodSourceCodeCollectionFactory = Substitute.For<IMethodSourceCodeCollectionFactory>();
            methodSourceCodeCollectionFactory.Create(methodsToOverload).Returns(expectedMethodSourceCodes);

            var expectedSourceCodeFiles = new SourceCodeFileCollection(
                new(new("SomeNamespace.SomeClass.g.cs"),
                    new("""
                        namespace SomeNamespace;

                        partial class SomeClass
                        {
                           void SomeMethod<T>() =>
                              SomeMethod<T>();
                        }
                        """)),
                new(new("SomeNamespace.SomeOtherClass.g.cs"),
                    new("""
                        namespace SomeNamespace;

                        partial class SomeOtherClass
                        {
                           int SomeOtherMethod<U>(int x) =>
                              SomeOtherMethod<U>(x);
                        }
                        """)));

            // Act
            var generatedFiles =
                new SourceCodeFileCollectionFactory(methodSourceCodeCollectionFactory, new SourceCodeFileFactory())
                    .Create(methodsToOverload);

            // Assert
            generatedFiles.Should().BeEquivalentTo(expectedSourceCodeFiles);
        }
    }
}