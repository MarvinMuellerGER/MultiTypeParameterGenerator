using MultiTypeParameterGenerator.Analysis.Factories.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Factories.Collections;

public static class AcceptedTypeCombinationCollectionFactoryTest
{
    public sealed class Create
    {
        [Fact]
        public void ReturnsAllAcceptedTypeCombinations()
        {
            // Arrange
            var acceptedTypesForAffectedGenericTypeCollection = new AcceptedTypesForAffectedGenericTypeCollection(
                new(new(new("T1")),
                    new(new(new NamedType(new(null, new("bool"))), false, false),
                        new(new NamedType(new(null, new("SomeRecord"))), true, false))),
                new(new(new("T3")),
                    new(new(new NamedType(new(null, new("double"))), false, false),
                        new(new NamedType(new(null, new("SomeRecord"))), true, false))));

            var methodToOverload = new MethodToOverload(
                false,
                true,
                false,
                new(new("Class"), new(new(new("SomeNamespace"), new("SomeClass"))), new()),
                new([new("public")]),
                new NamedType(new(null, new("void"))),
                new("SomeMethod"),
                new(),
                acceptedTypesForAffectedGenericTypeCollection,
                new([new(new NamedType(new(null, new("int"))), new("x"))]));

            var expectedCombinations = new AcceptedTypeCombinationCollection(
                new(
                    new(new(new("T1")), new(new NamedType(new(null, new("bool"))), false, false), false),
                    new(new(new("T3")), new(new NamedType(new(null, new("double"))), false, false), false)),
                new(
                    new(new(new("T1")), new(new NamedType(new(null, new("bool"))), false, false), false),
                    new(new(new("T3")), new(new NamedType(new(null, new("SomeRecord"))), true, false, 1), false)),
                new(
                    new(new(new("T1")), new(new NamedType(new(null, new("SomeRecord"))), true, false, 1), false),
                    new(new(new("T3")), new(new NamedType(new(null, new("double"))), false, false), false)),
                new(
                    new(new(new("T1")), new(new NamedType(new(null, new("SomeRecord"))), true, false, 1, 1), false),
                    new(new(new("T3")), new(new NamedType(new(null, new("SomeRecord"))), true, false, 2, 1), false)));

            // Act
            var result =
                new AcceptedTypeCombinationCollectionFactory().Create(methodToOverload);

            // Assert
            result.Values.Should().HaveCount(4, "two generics with two accepted types each should yield four combos");
            result.Should().BeEquivalentTo(expectedCombinations);
        }
    }
}