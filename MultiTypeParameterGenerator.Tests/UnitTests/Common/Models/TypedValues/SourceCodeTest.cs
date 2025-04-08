using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.TypedValues;

public class SourceCodeTest
{
    public class PlusOperator
    {
        [Fact]
        public void Should_MergeValues()
        {
            var first = new SourceCode("first ");
            var second = new SourceCode("second");
            var result = first + second;

            result.Value.Should().Be("first second");
        }
    }
}