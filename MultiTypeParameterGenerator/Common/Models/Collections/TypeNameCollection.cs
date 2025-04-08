using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Collections;

internal sealed record TypeNameCollection(params IReadOnlyList<TypeName> Values)
{
    internal SourceCode? SourceCode => Values.Any() ? new(Values.Join()) : null;
}