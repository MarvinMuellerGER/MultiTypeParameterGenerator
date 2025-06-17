using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Analysis.Factories.Collections;

internal sealed class ParameterCollectionFactory : IParameterCollectionFactory
{
    public ParameterCollection Create(
        MethodToOverload methodToOverload, AcceptedTypeCombination acceptedTypeCombination) =>
        methodToOverload.Parameters.WithAcceptedTypes(acceptedTypeCombination)
            .WithThisParameterIfNecessary(methodToOverload);
}