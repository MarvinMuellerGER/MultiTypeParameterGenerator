using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using static MultiTypeParameterGenerator.Common.Utils.Constants;

namespace MultiTypeParameterGenerator.Analysis.Models.Collections;

internal sealed record AccessModifierNameCollection(params IReadOnlyList<AccessModifierName> Values)
{
    internal bool IsEmpty => !Values.Any();

    internal SourceCode SourceCode => new(Values.Join(Whitespace));
}