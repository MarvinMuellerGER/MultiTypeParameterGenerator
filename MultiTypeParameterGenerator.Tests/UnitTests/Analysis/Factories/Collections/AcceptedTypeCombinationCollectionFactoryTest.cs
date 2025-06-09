using MultiTypeParameterGenerator.Analysis.Factories.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Factories.Collections;

public class AcceptedTypeCombinationCollectionFactoryTest
{
    public class Create
    {
        [Fact]
        public void Should_ReturnAllAcceptedTypeCombinations()
        {
            // Arrange
            var acceptedTypesForAffectedGenericTypeCollection = new AcceptedTypesForAffectedGenericTypeCollection(
                new(new(new("T1")),
                    new(new(new(null, new("bool")), false, false), new(new(null, new("string")), false, true))),
                new(new(new("T3")),
                    new(new(new(null, new("double")), false, true), new(new(null, new("string")), true, true))));

            var methodToOverload = new MethodToOverload(
                false,
                true,
                new(new("Class"), new(new("SomeNamespace"), new("SomeClass")), new()),
                new([new("public")]),
                new(null, new("void")),
                new("SomeMethod"),
                new(),
                acceptedTypesForAffectedGenericTypeCollection,
                new([new(new(null, new("int")), new("x"))]));

            var expectedCombinations = new AcceptedTypeCombinationCollection(
                new(
                    new(new(new("T1")), false, new(new(null, new("bool")), false, false, 1)),
                    new(new(new("T3")), false, new(new(null, new("double")), false, true, 1, 1))),
                new(
                    new(new(new("T1")), false, new(new(null, new("bool")), false, false, 1)),
                    new(new(new("T3")), true, new(new(null, new("string")), true, true, 1, 1))),
                new(
                    new(new(new("T1")), false, new(new(null, new("string")), false, true, 1, 1)),
                    new(new(new("T3")), false, new(new(null, new("double")), false, true, 1, 1))),
                new(
                    new(new(new("T1")), false, new(new(null, new("string")), false, true, 1, 1)),
                    new(new(new("T3")), true, new(new(null, new("string")), true, true, 1, 2))));

            // Act
            var result =
                new AcceptedTypeCombinationCollectionFactory().Create(methodToOverload);

            // Assert
            result.Values.Should().HaveCount(4, "two generics with two accepted types each should yield four combos");
            result.Should().BeEquivalentTo(expectedCombinations);
        }
    }
}