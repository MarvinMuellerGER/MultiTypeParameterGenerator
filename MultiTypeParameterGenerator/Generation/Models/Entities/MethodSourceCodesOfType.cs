using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Models.Collections;

namespace MultiTypeParameterGenerator.Generation.Models.Entities;

internal sealed record MethodSourceCodesOfType(MethodSourceCodeCollection MethodSourceCodes)
{
    private readonly MethodSourceCode _firstMethodSourceCode = MethodSourceCodes.Values.First();

    internal bool GenerateExtensionClass => _firstMethodSourceCode.GenerateExtensionMethod;

    internal ContainingType ContainingType => _firstMethodSourceCode.ContainingType;
}