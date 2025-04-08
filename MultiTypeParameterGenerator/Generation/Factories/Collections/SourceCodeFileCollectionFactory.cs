using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Factories.Entities;
using MultiTypeParameterGenerator.Generation.Models.Collections;

namespace MultiTypeParameterGenerator.Generation.Factories.Collections;

internal class SourceCodeFileCollectionFactory(
    IMethodSourceCodeCollectionFactory methodSourceCodeCollectionFactory,
    ISourceCodeFileFactory sourceCodeFileFactory) : ISourceCodeFileCollectionFactory
{
    public SourceCodeFileCollection Create(MethodToOverloadCollection methodsToOverload) =>
        Create(methodSourceCodeCollectionFactory.Create(methodsToOverload));

    private SourceCodeFileCollection Create(MethodSourceCodeCollection methodSourceCodes) =>
        new(methodSourceCodes.GroupedByContainingType.Values.SelectToReadonlyList(sourceCodeFileFactory.Create));
}