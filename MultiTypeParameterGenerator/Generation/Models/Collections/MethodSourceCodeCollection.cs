using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using MultiTypeParameterGenerator.Generation.Models.Entities;
using static MultiTypeParameterGenerator.Common.Utils.Constants;

namespace MultiTypeParameterGenerator.Generation.Models.Collections;

internal sealed record MethodSourceCodeCollection(params IReadOnlyList<MethodSourceCode> Values)
{
    internal bool AnyMethodToOverloadIsStatic => Values.Any(m => m.MethodToOverloadIsStatic);

    internal SourceCode SourceCode => new(Values.Select(m => m.SourceCode).Join(DoubleNewLine));

    internal MethodSourceCodesOfTypeCollection GroupedByContainingType =>
        new(Values.GroupBy(m => new { m.GenerateExtensionMethod, m.ContainingType.Name, m.ContainingType.Kind })
            .Select(g => new MethodSourceCodesOfType(new(g.ToList()))).ToList());
}