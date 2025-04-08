namespace MultiTypeParameterGenerator.Analysis.Extensions.Enums;

internal static class AccessModifierExtensions
{
    internal static string GetSourceCode(this AccessModifier accessModifier) =>
        accessModifier switch
        {
            AccessModifier.Public => "public",
            AccessModifier.Protected => "protected",
            AccessModifier.Internal => "internal",
            AccessModifier.ProtectedInternal => "protected internal",
            AccessModifier.PrivateProtected => "private protected",
            _ => throw new ArgumentOutOfRangeException(nameof(accessModifier), accessModifier, null)
        };
}