using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using Type = MultiTypeParameterGenerator.Common.Models.Entities.Type;

namespace MultiTypeParameterGenerator.Common.Models.Collections;

internal sealed record TypeCollection(params IReadOnlyList<Type> Values)
{
    internal FullTypeNameCollection FullTypeNames =>
        new(Values.SelectMany(t => t.FullTypeNames.Values).DistinctToReadonlyList());

    internal SourceCode SourceCode => GetSourceCode(t => t.SourceCode);
    internal SourceCode ShortenedSourceCode => GetSourceCode(t => t.ShortenedSourceCode);

    internal SourceCode ShortenedSourceCodeWithoutContainingType =>
        GetSourceCode(t => t.ShortenedSourceCodeWithoutContainingType);

    public override string ToString() => SourceCode.Value;

    internal TypeCollection WithAcceptedTypes(AcceptedTypeCombination acceptedTypeCombination) =>
        new(Values.SelectToReadonlyList(t => t.WithAcceptedTypes(acceptedTypeCombination)));

    private SourceCode GetSourceCode(Func<Type, SourceCode> selector) => new(Values.Select(selector.Invoke).Join());
}