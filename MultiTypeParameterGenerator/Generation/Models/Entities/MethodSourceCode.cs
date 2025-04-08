using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Generation.Models.Entities;

internal readonly record struct MethodSourceCode(
    bool GenerateExtensionMethod,
    ContainingType ContainingType,
    bool MethodToOverloadIsStatic,
    SourceCode SourceCode);