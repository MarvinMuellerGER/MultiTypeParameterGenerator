using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Models.Entities;

namespace MultiTypeParameterGenerator.Common.Factories.Entities;

internal interface ISourceCodeFileFactory
{
    SourceCodeFile Create(MethodSourceCodesOfType methodSourceCodesOfType);
}