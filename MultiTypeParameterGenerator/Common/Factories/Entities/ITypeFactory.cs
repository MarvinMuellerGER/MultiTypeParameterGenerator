using Microsoft.CodeAnalysis;
using Type = MultiTypeParameterGenerator.Common.Models.Entities.Type;

namespace MultiTypeParameterGenerator.Common.Factories.Entities;

internal interface ITypeFactory
{
    void SetContainingTypeOfMethod(INamedTypeSymbol containingTypeOfMethod);
    Type Create(ITypeSymbol type);
}