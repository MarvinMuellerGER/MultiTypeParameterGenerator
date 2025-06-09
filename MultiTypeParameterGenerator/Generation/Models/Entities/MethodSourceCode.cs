using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Generation.Models.Entities;

internal sealed record MethodSourceCode(
    bool UseFullTypeNames,
    bool GenerateExtensionMethod,
    ContainingType ContainingType,
    bool MethodToOverloadIsStatic,
    MethodName MethodName,
    SourceCode ParametersTypeNames,
    FullTypeNameCollection FullTypeNames,
    SourceCode SourceCode)
{
    internal NamespaceCollection NamespacesToImport => UseFullTypeNames
        ? new()
        : new(FullTypeNames.WithUniqueTypeNames.Values.SelectToReadonlyList(t => t.Namespace)
            .WhereNotNull()
            .OrderBy(n => n.Value)
            .WhereToReadonlyList(n =>
                n != ContainingType.Name.Namespace &&
                n.Value != ContainingType.Name.Value));

    internal NamespaceCollection NamespacesToRemove => UseFullTypeNames
        ? new()
        : new(FullTypeNames.Values.SelectToReadonlyList(t => t.Namespace)
            .Concat([new(ContainingType.NameInclGenericTypes.TypeName.Value)])
            .OrderByDescending(n => n?.Value.Length).WhereNotNull());

    internal FullTypeNameCollection FullTypeNamesWithTypeAlias
    {
        get
        {
            if (UseFullTypeNames)
                return FullTypeNames.WithoutUniqueTypeNames;

            var result = new List<FullTypeName>();

            var countByTypeName = new Dictionary<TypeName, int>();
            foreach (var fullTypeName in FullTypeNames.WithoutUniqueTypeNames.Values)
            {
                countByTypeName.TryGetValue(fullTypeName.TypeName, out var count);
                count++;
                countByTypeName[fullTypeName.TypeName] = count;

                result.Add(fullTypeName with
                {
                    Alias = new($"{fullTypeName.TypeName.Value}{(count > 1 ? count : string.Empty)}")
                });
            }

            return new(result);
        }
    }
}