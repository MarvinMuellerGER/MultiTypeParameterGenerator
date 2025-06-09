using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Models.TypedValues;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal sealed record AcceptedType(
    FullTypeName Name,
    bool IsNullable,
    bool UseTypeConstraint,
    int IndexOfAcceptedTypesWithSameShortName = 0,
    int IndexOfParametersWithSameType = 0)
{
    internal TypeName ShortName => Name.TypeName;

    internal SourceCode TypeNameInclNullableAnnotation => new(Name.TypeName + (IsNullable ? "?" : string.Empty));

    internal SourceCode NameForMethodName =>
        new(Name.TypeName.WithoutContainingType +
            (IndexOfAcceptedTypesWithSameShortName > 1 ? $"_{IndexOfAcceptedTypesWithSameShortName}" : string.Empty));

    internal SourceCode TypeNameForSourceCode =>
        UseTypeConstraint
            ? new($"T{Name.TypeName.WithoutContainingType}" +
                  (IndexOfAcceptedTypesWithSameShortName > 1
                      ? $"_{IndexOfAcceptedTypesWithSameShortName}"
                      : string.Empty) +
                  (IndexOfParametersWithSameType > 1 ? $"_{IndexOfParametersWithSameType}" : string.Empty))
            : TypeNameInclNullableAnnotation;
}