using MultiTypeParameterGenerator.Analysis.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.Entities;

public class ParameterTest
{
    public class SourceCodeProperty
    {
        [Fact]
        public void Should_ReturnTypeFollowedByName()
        {
            // Arrange
            var parameter = new Parameter(new("int"), new("myParam"));

            // Act & Assert
            parameter.SourceCode.Value.Should().Be("int myParam");
        }

        [Fact]
        public void Should_UseTypeNameForSourceCode()
        {
            // Arrange
            var parameter = new Parameter(new("int"), new("myParam")) { TypeNameForSourceCode = new("T1") };

            // Act & Assert
            parameter.SourceCode.Value.Should().Be("T1 myParam");
        }
    }
}