using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.Entities;

public static class ParameterTest
{
    public sealed class SourceCodeProperty
    {
        [Fact]
        public void ReturnsTypeFollowedByName()
        {
            // Arrange
            var parameter = new Parameter(new NamedType(new(null, new("int"))), new("myParam"));

            // Act & Assert
            parameter.SourceCode.Value.Should().Be("int myParam");
        }

        [Fact]
        public void UsesTypeNameForSourceCode()
        {
            // Arrange
            var parameter = new Parameter(new NamedType(new(null, new("int"))), new("myParam"))
                { TypeNameForSourceCode = new("T1") };

            // Act & Assert
            parameter.SourceCode.Value.Should().Be("T1 myParam");
        }
    }
}