using MultiTypeParameterGenerator.Common.Models.TypedValues;
using Type = MultiTypeParameterGenerator.Common.Models.Entities.Type;

namespace MultiTypeParameterGenerator.Analysis.Models.Entities;

internal sealed record AcceptedType(
    Type Type,
    bool UseTypeConstraint,
    int IndexOfParametersWithSameType = 0)
{
    internal TypeName ShortName => new(Type.ShortenedSourceCodeExclNullableAnnotation.Value);

    internal SourceCode TypeNameInclNullableAnnotation => Type.ShortenedSourceCode;

    internal SourceCode NameForMethodName => Type.ShortenedSourceCodeWithoutContainingType;

    internal SourceCode TypeNameForSourceCode =>
        UseTypeConstraint
            ? new($"T{Type.ShortenedSourceCodeWithoutContainingType}" +
                  (IndexOfParametersWithSameType > 1 ? $"_{IndexOfParametersWithSameType}" : string.Empty))
            : TypeNameInclNullableAnnotation;
}