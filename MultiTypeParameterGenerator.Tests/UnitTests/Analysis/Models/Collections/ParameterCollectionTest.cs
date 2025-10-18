using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.Collections;

public static class ParameterCollectionTest
{
    public sealed class NamesSourceCode
    {
        [Fact]
        public void ReturnsExpectedNames()
        {
            // Arrange
            var collection = new ParameterCollection(
                new(new NamedType(new(null, new("string"))), new("firstParam")),
                new(new NamedType(new(null, new("int"))), new("secondParam")));

            // Act & Assert
            collection.NamesSourceCode.Value.Should().Be("firstParam, secondParam");
        }
    }

    public sealed class SourceCodeProperty
    {
        [Fact]
        public void ReturnsJoinedParameterDeclarations()
        {
            // Arrange
            var collection = new ParameterCollection(
                new(new NamedType(new(null, new("string"))), new("firstParam")) { TypeNameForSourceCode = new("T1") },
                new(new NamedType(new(null, new("int"))), new("secondParam")));

            // Act & Assert
            collection.SourceCode.Value.Should().Be("T1 firstParam, int secondParam");
        }
    }

    public sealed class WithAcceptedTypesMethod
    {
        [Fact]
        public void ReplacesGenericTypesWithAcceptedTypes()
        {
            // Arrange
            var collection = new ParameterCollection(
                new(new GenericTypeParameter(new(null, new("T1"))), new("p1")),
                new(new GenericTypeParameter(new(null, new("T2"))), new("p2")));

            var combination = new AcceptedTypeCombination(
                new(new(new("T1")), false, new(new NamedType(new(null, new("bool")), null, new(), true), false)),
                new(new(new("T2")), false, new(new NamedType(new(null, new("SomeRecord"))), true)));

            // Act
            var updated = collection.WithAcceptedTypes(combination);

            // Assert
            var firstParam = updated.Values[0];
            var secondParam = updated.Values[1];

            firstParam.TypeNameForSourceCode.Value.Should().Be("bool?");
            secondParam.TypeNameForSourceCode.Value.Should().Be("TSomeRecord");
        }
    }

    public sealed class WithThisParameterIfNecessaryMethod
    {
        private static readonly MethodToOverload MethodToOverload = new(
            false,
            true,
            false,
            new(new("Class"), new(new(new("SomeNamespace"), new("SomeClass"))), new()),
            new([new("public")]),
            new NamedType(new(null, new("void"))),
            new("SomeMethod"),
            new(),
            new(),
            new([new(new NamedType(new(null, new("int"))), new("x"))]));

        [Fact]
        public void AddsThisParameter_WhenGenerateExtensionMethodIsTrue()
        {
            // Arrange
            var method = MethodToOverload with
            {
                GenerateExtensionMethod = true
            };

            // Act
            var updated = MethodToOverload.Parameters.WithThisParameterIfNecessary(method);

            // Assert
            updated.Values.Should().HaveCount(2);
            updated.Values[0].Name.Value.Should().Be("@this");
            updated.Values[0].Type.Should().Be(MethodToOverload.ContainingType.Type);
            updated.Values[1].Name.Value.Should().Be(MethodToOverload.Parameters.Values[0].Name.Value);
            updated.Values[1].Type.Should().Be(MethodToOverload.Parameters.Values[0].Type);
        }

        [Fact]
        public void DoesntAddThisParameter_WhenGenerateExtensionMethodIsFalse()
        {
            // Arrange
            var method = MethodToOverload with
            {
                GenerateExtensionMethod = false
            };

            // Act
            var updated = MethodToOverload.Parameters.WithThisParameterIfNecessary(method);

            // Assert
            updated.Values.Should().HaveCount(1);
            updated.Values[0].Name.Value.Should().Be(MethodToOverload.Parameters.Values[0].Name.Value);
            updated.Values[0].Type.Should().Be(MethodToOverload.Parameters.Values[0].Type);
        }
    }
}