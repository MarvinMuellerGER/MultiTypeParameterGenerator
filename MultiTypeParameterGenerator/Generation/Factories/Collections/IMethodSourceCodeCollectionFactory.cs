using MultiTypeParameterGenerator.Generation.Models.Collections;

namespace MultiTypeParameterGenerator.Generation.Factories.Collections;

internal interface IMethodSourceCodeCollectionFactory
{
    MethodSourceCodeCollection Create(MethodToOverloadCollection methodsToOverload);
}