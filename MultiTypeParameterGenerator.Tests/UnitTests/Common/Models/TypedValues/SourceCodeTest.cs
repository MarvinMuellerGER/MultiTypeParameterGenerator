using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.TypedValues;

public static class SourceCodeTest
{
    public sealed class PlusOperator
    {
        [Fact]
        public void MergesValues()
        {
            var first = new SourceCode("first ");
            var second = new SourceCode("second");
            var result = first + second;

            result.Value.Should().Be("first second");
        }
    }
}