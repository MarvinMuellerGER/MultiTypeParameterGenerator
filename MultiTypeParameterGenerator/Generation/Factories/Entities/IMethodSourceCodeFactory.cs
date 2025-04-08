using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Models.Entities;

namespace MultiTypeParameterGenerator.Generation.Factories.Entities;

internal interface IMethodSourceCodeFactory
{
    MethodSourceCode Create(MethodToOverload methodToOverload);
}