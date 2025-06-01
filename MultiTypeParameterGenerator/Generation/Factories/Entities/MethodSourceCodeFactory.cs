using MultiTypeParameterGenerator.Analysis.Factories.Collections;
using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;
using MultiTypeParameterGenerator.Generation.Extensions.Collections;
using MultiTypeParameterGenerator.Generation.Models.Entities;
using static MultiTypeParameterGenerator.Common.Utils.Constants;

namespace MultiTypeParameterGenerator.Generation.Factories.Entities;

internal class MethodSourceCodeFactory(
    IAcceptedTypeCombinationCollectionFactory acceptedTypeCombinationCollectionFactory,
    IParameterCollectionFactory parameterCollectionFactory) : IMethodSourceCodeFactory
{
    public MethodSourceCode Create(MethodToOverload methodToOverload) =>
        new(
            methodToOverload.GenerateExtensionMethod,
            methodToOverload.ContainingType,
            methodToOverload.MethodToOverloadIsStatic,
            methodToOverload.Name,
            methodToOverload.Parameters.TypeNamesSourceCode,
            GetSourceCode(methodToOverload));

    private SourceCode GetSourceCode(MethodToOverload methodToOverload) =>
        new(GetOverloadSourceCodes(methodToOverload).Join(DoubleNewLine));

    private IEnumerable<SourceCode> GetOverloadSourceCodes(MethodToOverload methodToOverload)
    {
        return acceptedTypeCombinationCollectionFactory.Create(methodToOverload).Values
            .Select(combination => GetOverloadSourceCode(methodToOverload, combination));
    }

    private SourceCode GetOverloadSourceCode(MethodToOverload methodToOverload,
        AcceptedTypeCombination acceptedTypeCombination) =>
        new(
            $"{Tab}{GetHeaderSourceCode(methodToOverload, acceptedTypeCombination)} =>{NewLine}{DoubleTab}{BodySourceCode(methodToOverload, acceptedTypeCombination)}");

    private SourceCode GetHeaderSourceCode(
        MethodToOverload methodToOverload, AcceptedTypeCombination acceptedTypeCombination) =>
        new(
            $"{GetAccessModifiersSourceCode(methodToOverload)}{GetReturnType(methodToOverload, acceptedTypeCombination)} {GetMethodOverloadName(methodToOverload, acceptedTypeCombination)}{GetTypeParametersAndParametersAndTypeConstraintsSourceCode(methodToOverload, acceptedTypeCombination)}");

    private static SourceCode GetAccessModifiersSourceCode(MethodToOverload methodToOverload) =>
        new(
            $"{(IsStaticModifierRequired(methodToOverload) ? "static " : "")}" +
            $"{methodToOverload.AccessModifiers.SourceCode}" +
            $"{(methodToOverload.AccessModifiers.IsEmpty ? "" : " ")}");

    private static SourceCode GetReturnType(
        MethodToOverload methodToOverload, AcceptedTypeCombination acceptedTypeCombination) =>
        new(methodToOverload.ReturnType.WithAcceptedTypes(acceptedTypeCombination).Value);

    private static MethodName GetMethodOverloadName(
        MethodToOverload methodToOverload, AcceptedTypeCombination acceptedTypeCombination) =>
        new($"{methodToOverload.Name.Value}{acceptedTypeCombination.Values
            .Where(acceptedType =>
                acceptedTypeCombination.AllUseTypeConstraints ||
                acceptedType is { IsNotFirstGenericAcceptedType: true, AcceptedType.UseTypeConstraint: true })
            .WithIsFirst()
            .Select(tuple => $"_{(tuple.IsFirst ? "With" : "And")}{tuple.Item.AcceptedType.NameForMethodName}")
            .Join(string.Empty)}");

    private SourceCode GetTypeParametersAndParametersAndTypeConstraintsSourceCode(
        MethodToOverload methodToOverload, AcceptedTypeCombination acceptedTypeCombination)
    {
        var genericTypes = methodToOverload.GenericTypes
            .ExceptNoneGenericAcceptedTypes(acceptedTypeCombination).WithGenericAcceptedTypes(acceptedTypeCombination);
        var parameters = parameterCollectionFactory.Create(methodToOverload, acceptedTypeCombination);

        return new(
            $"{genericTypes.SourceCode}({parameters.SourceCode}){genericTypes.ConstraintsSourceCode}");
    }

    private static SourceCode BodySourceCode(
        MethodToOverload methodToOverload, AcceptedTypeCombination acceptedTypeCombination) =>
        new(
            $"{GetThisParameterIfNecessary(methodToOverload)}{methodToOverload.Name}{GetTypeParametersSourceCode(methodToOverload, acceptedTypeCombination)}({GetParameterNamesSourceCode(methodToOverload)});");

    private static SourceCode GetThisParameterIfNecessary(MethodToOverload methodToOverload) =>
        methodToOverload.GenerateExtensionMethod ? new("@this.") : new();

    private static SourceCode GetTypeParametersSourceCode(
        MethodToOverload methodToOverload, AcceptedTypeCombination acceptedTypeCombination) =>
        GetTypeParameters(methodToOverload, acceptedTypeCombination).SourceCode;

    private static GenericTypeCollection GetTypeParameters(
        MethodToOverload methodToOverload, AcceptedTypeCombination acceptedTypeCombination) =>
        methodToOverload.GenericTypes.WithAcceptedTypes(acceptedTypeCombination);

    private static SourceCode GetParameterNamesSourceCode(MethodToOverload methodToOverload) =>
        methodToOverload.Parameters.NamesSourceCode;

    private static bool IsStaticModifierRequired(MethodToOverload methodToOverload) => methodToOverload is
        { GenerateExtensionMethod: true, MethodToOverloadIsStatic: false };
}