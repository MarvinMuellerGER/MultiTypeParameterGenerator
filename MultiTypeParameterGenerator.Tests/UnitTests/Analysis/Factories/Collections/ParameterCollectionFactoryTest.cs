using MultiTypeParameterGenerator.Analysis.Factories.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Factories.Collections;

public static class ParameterCollectionFactoryTest
{
    public sealed class Create
    {
        [Fact]
        public void UpdatesParameters_BasedOnAcceptedTypeCombination()
        {
            // Arrange
            var methodToOverload = new MethodToOverload(
                false,
                false,
                false,
                new(new("class"), new(new(new("SomeNamespace"), new("SomeClass"))), new()),
                new([new("public")]),
                new NamedType(new(null, new("void"))),
                new("SomeMethod"),
                new(),
                new(),
                new([new(new NamedType(new(null, new("T1"))), new("value"))]));

            var acceptedTypeCombination =
                new AcceptedTypeCombination([
                    new(new(new("T1")), false, new(new NamedType(new(null, new("int"))), false))
                ]);

            // Act
            var result = new ParameterCollectionFactory().Create(methodToOverload, acceptedTypeCombination);

            // Assert
            result.Should().NotBeNull();
            result.Values.Should().ContainSingle()
                .Which.TypeNameForSourceCode.Value.Should().Be("int");
        }
    }
}