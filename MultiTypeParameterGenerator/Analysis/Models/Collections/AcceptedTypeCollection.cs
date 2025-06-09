using MultiTypeParameterGenerator.Analysis.Models.Entities;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Collections;

namespace MultiTypeParameterGenerator.Analysis.Models.Collections;

internal sealed record AcceptedTypeCollection(params IReadOnlyList<AcceptedType> Values)
{
    internal FullTypeNameCollection FullTypeNames => new(Values.Select(a => a.Name));

    internal NamespaceCollection Namespaces => new(Values.Select(a => a.Name.Namespace).WhereNotNull());
}