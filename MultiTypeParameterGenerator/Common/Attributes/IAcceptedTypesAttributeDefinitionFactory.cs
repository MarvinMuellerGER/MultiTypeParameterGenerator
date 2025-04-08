using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Common.Attributes;

internal interface IAcceptedTypesAttributeDefinitionFactory
{
    AttributeDefinition Create();
}