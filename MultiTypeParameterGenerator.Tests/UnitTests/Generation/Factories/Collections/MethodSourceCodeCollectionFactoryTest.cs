using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Extensions.Collections;
using MultiTypeParameterGenerator.Generation.Factories.Collections;
using MultiTypeParameterGenerator.Generation.Factories.Entities;
using MultiTypeParameterGenerator.Generation.Models.Collections;
using NSubstitute;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Generation.Factories.Collections;

public class MethodSourceCodeCollectionFactoryTest
{
    public class Create
    {
        [Fact]
        public void Should_CreateMethodSourceCodeCollectionFromMethodToOverloadCollection()
        {
            // Arrange
            var methodsToOverload = new MethodToOverloadCollection(
                new(
                    false,
                    new(new("class"), new(new("SomeNamespace"), new("SomeClass")), new()),
                    new([new("public")]),
                    new(new("void")),
                    new("SomeMethod"),
                    new([new(new("T"))]),
                    new(),
                    new([new(new("int"), new("x"))])),
                new(
                    false,
                    new(new("class"), new(new("SomeNamespace"), new("SomeClass")), new()),
                    new([new("public")]),
                    new(new("void")),
                    new("SomeOtherMethod"),
                    new([new(new("U"))]),
                    new(),
                    new([new(new("string"), new("s"))])));

            var methodSourceCodeFactory = Substitute.For<IMethodSourceCodeFactory>();
            methodSourceCodeFactory.Create(Arg.Any<MethodToOverload>())
                .Returns(args =>
                    new(
                        false,
                        new(new("class"), new(new("SomeNamespace"), new("SomeClass")), new()),
                        false,
                        args.Arg<MethodToOverload>() == methodsToOverload.Values[0]
                            ? new("""
                                     public void SomeMethod<T>(int x) =>
                                        SomeMethod<T>(x);
                                  """)
                            : new("""
                                     public void SomeOtherMethod<U>(string s) =>
                                        SomeOtherMethod<U>(s);
                                  """)));

            var expectedMethodSourceCodes =
                methodsToOverload.Values.Select(methodSourceCodeFactory.Create).ToReadonlyList();

            // Act
            var sourceCodes = new MethodSourceCodeCollectionFactory(methodSourceCodeFactory).Create(methodsToOverload);

            // Assert
            sourceCodes.Values.Should().BeEquivalentTo(expectedMethodSourceCodes);
        }
    }
}