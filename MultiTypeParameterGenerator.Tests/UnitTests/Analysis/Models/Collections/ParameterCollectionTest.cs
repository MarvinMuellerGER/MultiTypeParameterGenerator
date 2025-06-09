using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.Collections;

public class ParameterCollectionTest
{
    public class NamesSourceCode
    {
        [Fact]
        public void Should_ReturnExpectedNames()
        {
            // Arrange
            var collection = new ParameterCollection(
                new(new(null, new("string")), new("firstParam")),
                new(new(null, new("int")), new("secondParam")));

            // Act & Assert
            collection.NamesSourceCode.Value.Should().Be("firstParam, secondParam");
        }
    }

    public class SourceCodeProperty
    {
        [Fact]
        public void Should_ReturnJoinedParameterDeclarations()
        {
            // Arrange
            var collection = new ParameterCollection(
                new(new(null, new("string")), new("firstParam")) { TypeNameForSourceCode = new("T1") },
                new(new(null, new("int")), new("secondParam")));

            // Act & Assert
            collection.SourceCode.Value.Should().Be("T1 firstParam, int secondParam");
        }
    }

    public class WithAcceptedTypesMethod
    {
        [Fact]
        public void Should_ReplaceGenericTypesWithAcceptedTypes()
        {
            // Arrange
            var collection = new ParameterCollection(
                new(new(null, new("T1")), new("p1")),
                new(new(null, new("T2")), new("p2")));

            var combination = new AcceptedTypeCombination(
                new(new(new("T1")), false, new(new(null, new("bool")), true, false)),
                new(new(new("T2")), false, new(new(null, new("SomeRecord")), false, true)));

            // Act
            var updated = collection.WithAcceptedTypes(combination);

            // Assert
            var firstParam = updated.Values[0];
            var secondParam = updated.Values[1];

            firstParam.TypeNameForSourceCode.Value.Should().Be("bool?");
            secondParam.TypeNameForSourceCode.Value.Should().Be("TSomeRecord");
        }
    }

    public class WithThisParameterIfNecessaryMethod
    {
        private static readonly MethodToOverload MethodToOverload = new(
            false,
            true,
            new(new("Class"), new(new("SomeNamespace"), new("SomeClass")), new()),
            new([new("public")]),
            new(null, new("void")),
            new("SomeMethod"),
            new(),
            new(),
            new([new(new(null, new("int")), new("x"))]));

        [Fact]
        public void Should_AddThisParameter_WhenGenerateExtensionMethodIsTrue()
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
            updated.Values[0].Type.Value.Should().Be(MethodToOverload.ContainingType.Name.Value);
            updated.Values[1].Name.Value.Should().Be(MethodToOverload.Parameters.Values[0].Name.Value);
            updated.Values[1].Type.Value.Should().Be(MethodToOverload.Parameters.Values[0].Type.Value);
        }

        [Fact]
        public void Should_NotAddThisParameter_WhenGenerateExtensionMethodIsFalse()
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
            updated.Values[0].Type.Value.Should().Be(MethodToOverload.Parameters.Values[0].Type.Value);
        }
    }
}