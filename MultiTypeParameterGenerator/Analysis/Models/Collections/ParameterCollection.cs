using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using static MultiTypeParameterGenerator.Common.Utils.Constants;
using static MultiTypeParameterGenerator.Analysis.Extensions.Collections.EnumerableExtensions;

namespace MultiTypeParameterGenerator.Analysis.Models.Collections;

internal sealed record ParameterCollection(params IReadOnlyList<Parameter> Values)
{
    internal SourceCode NamesSourceCode => new(Values.Select(p => p.Name).Join());

    internal SourceCode TypeNamesSourceCode => new(Values.Select(p => p.TypeNameForSourceCode).Join());

    internal SourceCode SourceCode => new(Values.Select(p => p.SourceCode).Join());

    internal FullTypeNameCollection FullTypeNames =>
        new(Values.SelectManyToReadonlyList(p => p.Type.FullTypeNames.Values));

    internal ParameterCollection WithAcceptedTypes(AcceptedTypeCombination acceptedTypeCombination) =>
        new(Values.ReplaceAll(p =>
            p with
            {
                TypeNameForSourceCode = p.Type.WithAcceptedTypes(acceptedTypeCombination).ShortenedSourceCode
            }));

    internal ParameterCollection WithThisParameterIfNecessary(MethodToOverload methodToOverload) =>
        methodToOverload.GenerateExtensionMethod ? WithThisParameter(methodToOverload) : this;

    private ParameterCollection WithThisParameter(MethodToOverload methodToOverload) =>
        new(Values.Prepend(new(
            methodToOverload.ContainingType.Type,
            new(ThisIdentifier))).ToList());
}