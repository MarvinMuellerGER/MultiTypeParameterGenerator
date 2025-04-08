using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Analysis.Factories.Collections;

internal interface IAcceptedTypeCombinationCollectionFactory
{
    AcceptedTypeCombinationCollection Create(MethodToOverload methodToOverload);
}