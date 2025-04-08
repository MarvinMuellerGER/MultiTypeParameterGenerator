using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Common.Attributes;

internal interface IAttributesDefinition
{
    AttributeDefinition AccessModifiersAttributeDefinition { get; }
    AttributeDefinition AcceptedTypesAttributeDefinition { get; }
    AttributeDefinitionCollection AttributeDefinitions { get; }
}