using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Common.Models.Collections;

internal sealed record GenericTypeCollection(params IReadOnlyList<GenericType> Values)
{
    internal SourceCode SourceCode => new(Values.Any() ? $"<{Values.Join()}>" : string.Empty);

    internal SourceCode SourceCodeWithoutBrackets => new(Values.Join());

    internal SourceCode ConstraintsSourceCode =>
        new(Values.Select(genericType => genericType.ConstraintSourceCode).WhereNotNull().Join(string.Empty));

    internal GenericTypeCollection ExceptNoneGenericAcceptedTypes(AcceptedTypeCombination acceptedTypeCombination) =>
        Except(acceptedTypeCombination.AffectedGenericTypesWithoutGenericAcceptedTypes);

    private GenericTypeCollection Except(GenericTypeCollection other) =>
        new(Values.WhereToReadonlyList(gt => other.Values.All(o => o.Name != gt.Name)));

    internal GenericTypeCollection WithAcceptedTypes(AcceptedTypeCombination acceptedTypeCombination) =>
        acceptedTypeCombination.Values.Aggregate(this,
            (current, acceptedType) => current.Replace(acceptedType.AffectedGenericType,
                GenericType.FromAcceptedType(acceptedType.AcceptedType)));

    internal GenericTypeCollection WithGenericAcceptedTypes(AcceptedTypeCombination acceptedTypeCombination) =>
        acceptedTypeCombination.Values.Aggregate(this,
            (current, acceptedType) => current.Replace(acceptedType.AffectedGenericType,
                GenericType.FromAcceptedType(acceptedType.AcceptedType)));

    private GenericTypeCollection Replace(GenericType valueToReplace, GenericType newValue)
    {
        var list = Values.ToList();

        var genericTypeToReplace = list.FirstOrDefault(gt => gt.Name == valueToReplace.Name);
        var index = list.IndexOf(genericTypeToReplace);
        if (index is -1)
            return this;
        list[index] = newValue;

        return new(list);
    }
}