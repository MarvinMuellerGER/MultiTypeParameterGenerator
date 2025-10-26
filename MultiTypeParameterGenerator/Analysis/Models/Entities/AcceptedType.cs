using MultiTypeParameterGenerator.Common.Models.TypedValues;
using Type = MultiTypeParameterGenerator.Common.Models.Entities.Type;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal sealed record AcceptedType(
    Type Type,
    bool UseTypeConstraint,
    bool UseFullTypeName,
    int IndexOfParametersWithSameType = 0,
    int IndexOfParametersWithSameShortTypeName = 0)
{
    internal SourceCode NameForMethodName => new(Type.ShortenedSourceCodeWithoutContainingType +
                                                 (IndexOfParametersWithSameShortTypeName > 1
                                                     ? $"_{IndexOfParametersWithSameShortTypeName}"
                                                     : string.Empty));

    internal SourceCode TypeNameForSourceCode =>
        UseTypeConstraint
            ? new($"T{Type.ShortenedSourceCodeWithoutContainingType}" +
                  (IndexOfParametersWithSameType > 1 ? $"_{IndexOfParametersWithSameType}" : string.Empty))
            : UseFullTypeName
                ? Type.SourceCode
                : Type.ShortenedSourceCode;
}