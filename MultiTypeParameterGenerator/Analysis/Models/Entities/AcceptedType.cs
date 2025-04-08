using MultiTypeParameterGenerator.Analysis.Models.TypedValues;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal sealed record AcceptedType(
    AcceptedTypeName Name,
    bool IsNullable,
    bool UseTypeConstraint,
    int IndexOfAcceptedTypesWithSameShortName = 0,
    int IndexOfParametersWithSameType = 0)
{
    internal AcceptedTypeName ShortName => Name.ShortName;

    internal SourceCode NameInclNullableAnnotation => new(Name.Value + (IsNullable ? "?" : string.Empty));

    internal SourceCode NameForMethodName =>
        new(Name.ShortName +
            (IndexOfAcceptedTypesWithSameShortName > 1 ? $"_{IndexOfAcceptedTypesWithSameShortName}" : string.Empty));

    internal SourceCode TypeNameForSourceCode =>
        UseTypeConstraint
            ? new($"T{ShortName}" +
                  (IndexOfAcceptedTypesWithSameShortName > 1
                      ? $"_{IndexOfAcceptedTypesWithSameShortName}"
                      : string.Empty) +
                  (IndexOfParametersWithSameType > 1 ? $"_{IndexOfParametersWithSameType}" : string.Empty))
            : NameInclNullableAnnotation;
}