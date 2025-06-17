using MultiTypeParameterGenerator.Common.Models.Collections;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Models.Collections;

public static class AttributeDefinitionCollectionTest
{
    public sealed class SourceCodeFiles
    {
        [Fact]
        public void ContainsAllSourceCodeFilesFromAttributeDefinitions()
        {
            // Arrange
            var collection = new AttributeDefinitionCollection(new(
                new(new("SomeNamespace"), new("SomeClass")),
                new(new("SomeClass.cs"), new("SomeClassAttribute"))
            ), new(
                new(new("SomeNamespace"), new("SomeOtherClass")),
                new(new("SomeOtherClass.cs"), new("SomeOtherClassAttribute"))
            ));

            // Act
            var sourceFiles = collection.SourceCodeFiles.Values;

            // Assert
            sourceFiles.Should().HaveCount(2);
            sourceFiles[0].FileName.Value.Should().Be("SomeClass.cs");
            sourceFiles[1].FileName.Value.Should().Be("SomeOtherClass.cs");
        }
    }
}