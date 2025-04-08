using MultiTypeParameterGenerator.Generation.Extensions.Collections;
using MultiTypeParameterGenerator.Generation.Factories.Entities;
using MultiTypeParameterGenerator.Generation.Models.Collections;

namespace MultiTypeParameterGenerator.Generation.Factories.Collections;

internal class MethodSourceCodeCollectionFactory(IMethodSourceCodeFactory methodSourceCodeFactory)
    : IMethodSourceCodeCollectionFactory
{
    public MethodSourceCodeCollection Create(MethodToOverloadCollection methodsToOverload) =>
        new(methodsToOverload.Values.Select(methodSourceCodeFactory.Create).ToReadonlyList());
}