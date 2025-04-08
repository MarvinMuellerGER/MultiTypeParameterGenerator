using System.Runtime.CompilerServices;

namespace MultiTypeParameterGenerator.Tests;

internal static class ModuleInitializer
{
    [ModuleInitializer]
    internal static void Initialize() =>
        VerifyDiffPlex.Initialize();
}