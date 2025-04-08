using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.TypedValues;

public class ParameterTypeNameTest
{
    public class Equality
    {
        [Fact]
        public void ShouldBe_EqualToGenericTypeNameIfSameValue()
        {
            // Arrange
            var paramTypeName = new ParameterTypeName("T1");
            var genericTypeName = new GenericTypeName("T1");

            // Act & Assert
            (paramTypeName == genericTypeName).Should().BeTrue();
            (paramTypeName != genericTypeName).Should().BeFalse();
        }

        [Fact]
        public void ShouldNotBe_EqualIfValueIsDifferent()
        {
            // Arrange
            var paramTypeName = new ParameterTypeName("T1");
            var genericTypeName = new GenericTypeName("T2");

            // Act & Assert
            (paramTypeName == genericTypeName).Should().BeFalse();
            (paramTypeName != genericTypeName).Should().BeTrue();
        }
    }
}