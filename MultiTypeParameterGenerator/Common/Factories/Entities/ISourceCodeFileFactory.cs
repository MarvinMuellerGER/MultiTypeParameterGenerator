using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Common.Factories.Entities;

internal interface ISourceCodeFileFactory
{
    SourceCodeFile Create(MethodToOverload methodToOverload);
}