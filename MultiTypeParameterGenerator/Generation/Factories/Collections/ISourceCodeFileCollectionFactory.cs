using MultiTypeParameterGenerator.Generation.Models.Collections;

namespace MultiTypeParameterGenerator.Generation.Factories.Collections;

internal interface ISourceCodeFileCollectionFactory
{
    SourceCodeFileCollection Create(MethodToOverloadCollection methodsToOverload);
}