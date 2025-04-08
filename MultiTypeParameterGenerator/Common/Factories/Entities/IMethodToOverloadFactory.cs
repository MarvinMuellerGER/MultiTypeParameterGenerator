using Microsoft.CodeAnalysis;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Common.Factories.Entities;

internal interface IMethodToOverloadFactory
{
    MethodToOverload? Create(IMethodSymbol method);
}