using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Models.Collections;

namespace MultiTypeParameterGenerator.Common.Models.Collections;

internal sealed record AttributeDefinitionCollection(params IReadOnlyList<AttributeDefinition> Values)
{
    internal SourceCodeFileCollection SourceCodeFiles => new(Values.SelectToReadonlyList(a => a.SourceCodeFile));
}