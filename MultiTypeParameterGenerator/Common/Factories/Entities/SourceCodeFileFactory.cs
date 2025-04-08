using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using MultiTypeParameterGenerator.Generation.Models.Entities;
using MultiTypeParameterGenerator.Generation.Models.TypedValues;
using static MultiTypeParameterGenerator.Common.Utils.Constants;

namespace MultiTypeParameterGenerator.Common.Factories.Entities;

internal class SourceCodeFileFactory : ISourceCodeFileFactory
{
    public SourceCodeFile Create(MethodSourceCodesOfType methodSourceCodesOfType) =>
        new(GetFileName(methodSourceCodesOfType), GetSourceCode(methodSourceCodesOfType));

    private static FileName GetFileName(MethodSourceCodesOfType methodSourceCodesOfType) =>
        new($"{methodSourceCodesOfType.ContainingType.Name}{GetPrefix(methodSourceCodesOfType)}.g.cs");

    private static SourceCode GetSourceCode(MethodSourceCodesOfType methodSourceCodesOfType) =>
        new($$"""
              {{GetUsings(methodSourceCodesOfType)}}{{GetNamespace(methodSourceCodesOfType)}}

              {{GetContainingTypeKindWithAccessModifiers(methodSourceCodesOfType)}} {{GetContainingTypeName(methodSourceCodesOfType)}}
              {
              {{methodSourceCodesOfType.MethodSourceCodes.SourceCode}}
              }
              """);

    private static SourceCode GetUsings(MethodSourceCodesOfType methodSourceCodesOfType) =>
        new(IsStaticUsingOfContainingTypeRequired(methodSourceCodesOfType)
            ? $"{NewLine}using static {methodSourceCodesOfType.ContainingType};{NewLine}"
            : string.Empty);

    private static SourceCode GetNamespace(MethodSourceCodesOfType methodSourceCodesOfType) =>
        new($"namespace {methodSourceCodesOfType.ContainingType.Name.Namespace};");

    private static SourceCode
        GetContainingTypeKindWithAccessModifiers(MethodSourceCodesOfType methodSourceCodesOfType) =>
        new(methodSourceCodesOfType.GenerateExtensionClass
            ? "public static class"
            : $"partial {methodSourceCodesOfType.ContainingType.Kind}");

    private static SourceCode GetContainingTypeName(MethodSourceCodesOfType methodSourceCodesOfType) =>
        new(
            $"{methodSourceCodesOfType.ContainingType.Name.TypeName}{GetPrefix(methodSourceCodesOfType)}{GetGenericTypes(methodSourceCodesOfType)}{GetTypeConstraints(methodSourceCodesOfType)}");

    private static SourceCode GetGenericTypes(MethodSourceCodesOfType methodSourceCodesOfType) =>
        methodSourceCodesOfType.ContainingType.GenericTypes.SourceCode;

    private static SourceCode GetTypeConstraints(MethodSourceCodesOfType methodSourceCodesOfType) =>
        methodSourceCodesOfType.GenerateExtensionClass
            ? methodSourceCodesOfType.ContainingType.GenericTypes.ConstraintsSourceCode
            : new();

    private static SourceCode GetPrefix(MethodSourceCodesOfType methodSourceCodesOfType) =>
        new(methodSourceCodesOfType.GenerateExtensionClass
            ? "Extensions"
            : string.Empty);

    private static bool IsStaticUsingOfContainingTypeRequired(MethodSourceCodesOfType methodSourceCodesOfType) =>
        methodSourceCodesOfType is
            { GenerateExtensionClass: true, MethodSourceCodes.AnyMethodToOverloadIsStatic: true };
}