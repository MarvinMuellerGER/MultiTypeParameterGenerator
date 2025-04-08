using System.Reflection;
using Microsoft.CodeAnalysis;
using MultiTypeParameterGenerator.Common.Extensions.Roslyn;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Models.Collections;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Extensions.Roslyn;

public class IncrementalGeneratorPostInitializationContextExtensionsTest
{
    private const BindingFlags NonPublicBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
    private static readonly Type ContextType = typeof(IncrementalGeneratorPostInitializationContext);

    private static readonly Type AdditionalSourcesCollectionType =
        ContextType.Assembly.GetType("Microsoft.CodeAnalysis.AdditionalSourcesCollection")!;

    private static readonly MethodInfo? ContainsMethodInfo =
        AdditionalSourcesCollectionType.GetMethod("Contains", [typeof(string)]);

    [Fact]
    public void Should_AddSourcesForEachFileInCollection()
    {
        // Arrange
        var context = CreateIncrementalGeneratorPostInitializationContext();
        var files = new SourceCodeFileCollection
        (
            new SourceCodeFile(new("TestFile1.cs"), new("content1")),
            new SourceCodeFile(new("TestFile2.cs"), new("content2"))
        );

        // Act
        context.AddSources(files);

        // Assert
        var additionalSources = GetAdditionalSources(context);
        additionalSources.Should().NotBeNull();
        Contains(additionalSources, "TestFile1.cs").Should().BeTrue();
        Contains(additionalSources, "TestFile2.cs").Should().BeTrue();
    }

    private static IncrementalGeneratorPostInitializationContext CreateIncrementalGeneratorPostInitializationContext()
    {
        var additionalSourcesCollection = Activator.CreateInstance(
            AdditionalSourcesCollectionType,
            NonPublicBindingFlags,
            null,
            [".cs"],
            null
        )!;

        return (IncrementalGeneratorPostInitializationContext)Activator.CreateInstance(
            ContextType,
            NonPublicBindingFlags,
            null,
            [additionalSourcesCollection, CancellationToken.None],
            null
        )!;
    }

    private static object? GetAdditionalSources(IncrementalGeneratorPostInitializationContext context) =>
        ContextType.GetField("AdditionalSources", NonPublicBindingFlags)!.GetValue(context);

    private static bool Contains(object additionalSources, string hintName) =>
        (bool)ContainsMethodInfo!.Invoke(additionalSources, [hintName])!;
}