using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Extensions;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using MultiTypeParameterGenerator.Generation.Factories.Entities;
using MultiTypeParameterGenerator.Generation.Models.Entities;
using MultiTypeParameterGenerator.Generation.Models.TypedValues;
using static MultiTypeParameterGenerator.Common.Utils.Constants;
using static MultiTypeParameterGenerator.Common.Extensions.Collections.EnumerableExtensions;

namespace MultiTypeParameterGenerator.Common.Factories.Entities;

internal class SourceCodeFileFactory(IMethodSourceCodeFactory methodSourceCodeFactory) : ISourceCodeFileFactory
{
    private static readonly Dictionary<FileName, int> CountByFileName = [];

    private static readonly Dictionary<(FileName FileName, SourceCode ParametersTypeNames), int>
        IndexByFileNameAndParameters = [];

    public SourceCodeFile Create(MethodToOverload methodToOverload) =>
        Create(methodSourceCodeFactory.Create(methodToOverload));

    private static SourceCodeFile Create(MethodSourceCode methodSourceCode) =>
        new(GetFileName(methodSourceCode), GetSourceCode(methodSourceCode));

    private static FileName GetFileName(MethodSourceCode methodSourceCode)
    {
        var fileNameWithoutExtension = new FileName(
            $"{methodSourceCode.ContainingType.FullNameWithGenericTypesCount}{GetPrefix(methodSourceCode)}.{GetMethodNameForFileName(methodSourceCode)}");

        var key = (fileNameWithoutExtension, methodSourceCode.ParametersTypeNames);
        if (!IndexByFileNameAndParameters
                .TryGetValue(key, out var index))
        {
            CountByFileName.TryGetValue(fileNameWithoutExtension, out var count);
            index = count;
            CountByFileName[fileNameWithoutExtension] = count + 1;
            IndexByFileNameAndParameters[key] = index;
        }

        var countPrefix = index is 0 ? string.Empty : $"_{index + 1}";
        return new($"{fileNameWithoutExtension}{countPrefix}.g.cs");
    }

    private static MethodName GetMethodNameForFileName(MethodSourceCode methodSourceCode) =>
        methodSourceCode.MethodName;

    private static SourceCode GetSourceCode(MethodSourceCode methodSourceCode) =>
        new($$"""
              {{GetUsings(methodSourceCode)}}{{GetAliasUsings(methodSourceCode)}}{{GetStaticUsingOfContainingTypeIfRequired(methodSourceCode)}}{{GetNamespace(methodSourceCode)}}

              {{GetContainingTypeKindWithAccessModifiers(methodSourceCode)}} {{GetContainingTypeName(methodSourceCode)}}
              {
              {{methodSourceCode.SourceCode.Value.RemoveMultiple(GetNamespacesToRemove(methodSourceCode))}}
              }
              """);

    private static SourceCode GetNamespace(MethodSourceCode methodSourceCode) =>
        new($"namespace {methodSourceCode.ContainingType.Name.Namespace};");

    private static SourceCode
        GetContainingTypeKindWithAccessModifiers(MethodSourceCode methodSourceCode) =>
        new(methodSourceCode.GenerateExtensionMethod
            ? "public static class"
            : $"partial {methodSourceCode.ContainingType.Kind}");

    private static SourceCode GetContainingTypeName(MethodSourceCode methodSourceCode) =>
        new(
            $"{methodSourceCode.ContainingType.Name.TypeName}{GetPrefix(methodSourceCode)}{GetGenericTypes(methodSourceCode)}{GetTypeConstraints(methodSourceCode)}");

    private static IReadOnlyList<string> GetNamespacesToRemove(MethodSourceCode methodSourceCode) =>
        methodSourceCode.NamespacesToRemove.Values.SelectToReadonlyList(n => $"{n}.");

    private static SourceCode GetGenericTypes(MethodSourceCode methodSourceCode) =>
        methodSourceCode.ContainingType.GenericTypes.SourceCode;

    private static SourceCode GetTypeConstraints(MethodSourceCode methodSourceCode) =>
        methodSourceCode.GenerateExtensionMethod
            ? methodSourceCode.ContainingType.GenericTypes.ConstraintsSourceCode
            : new();

    private static SourceCode GetPrefix(MethodSourceCode methodSourceCode) =>
        new(methodSourceCode.GenerateExtensionMethod
            ? "Extensions"
            : string.Empty);

    private static SourceCode GetUsings(MethodSourceCode methodSourceCode) =>
        new(methodSourceCode.NamespacesToImport.Values.Any()
            ? $"""
               {methodSourceCode.NamespacesToImport.Values.Select(n => $"using {n};").Join(NewLine)}


               """
            : string.Empty);

    private static SourceCode GetAliasUsings(MethodSourceCode methodSourceCode) =>
        new(methodSourceCode.FullTypeNamesWithTypeAlias.Values.Any()
            ? $"""
               {methodSourceCode.FullTypeNamesWithTypeAlias.Values.Select(t => $"using {t.Alias} = {t};").Join(NewLine)}


               """
            : string.Empty);

    private static SourceCode GetStaticUsingOfContainingTypeIfRequired(MethodSourceCode methodSourceCode) =>
        new(IsStaticUsingOfContainingTypeRequired(methodSourceCode)
            ? $"""
               using static {methodSourceCode.ContainingType};


               """
            : string.Empty);

    private static bool IsStaticUsingOfContainingTypeRequired(MethodSourceCode methodSourceCode) =>
        methodSourceCode is
            { GenerateExtensionMethod: true, MethodToOverloadIsStatic: true };
}