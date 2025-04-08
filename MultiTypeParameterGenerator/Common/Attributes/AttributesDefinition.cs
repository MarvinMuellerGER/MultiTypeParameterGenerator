using MultiTypeParameterGenerator.Common.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;

namespace MultiTypeParameterGenerator.Common.Attributes;

internal class AttributesDefinition(
    IAccessModifiersAttributeDefinitionFactory accessModifiersAttributeDefinitionFactory,
    IAcceptedTypesAttributeDefinitionFactory acceptedTypesAttributeDefinitionFactory)
    : IAttributesDefinition
{
    public AttributeDefinition AccessModifiersAttributeDefinition { get; } =
        accessModifiersAttributeDefinitionFactory.Create();

    public AttributeDefinition AcceptedTypesAttributeDefinition { get; } =
        acceptedTypesAttributeDefinitionFactory.Create();

    public AttributeDefinitionCollection AttributeDefinitions =>
        new(AccessModifiersAttributeDefinition, AcceptedTypesAttributeDefinition);
}