using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Analysis.Factories.Collections;

internal interface IParameterCollectionFactory
{
    ParameterCollection Create(
        MethodToOverload methodToOverload, AcceptedTypeCombination acceptedTypeCombination);
}