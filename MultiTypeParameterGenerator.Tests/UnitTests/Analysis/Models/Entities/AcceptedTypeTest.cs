using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Analysis.Models.Entities;

public static class AcceptedTypeTest
{
    public sealed class TypeNameForSourceCode
    {
        [Fact]
        public void ReturnsTypeName_WhenUseTypeConstraintIsFalse()
        {
            // Arrange
            var acceptedType = new AcceptedType(new NamedType(new(new("SomeNamespace"), new("SomeClass"))), false, false);

            // Act & Assert
            acceptedType.TypeNameForSourceCode.Value.Should()
                .BeEquivalentTo(acceptedType.Type.FullTypeNames.Values[0].TypeName.Value);
        }
    }
}