using System.Diagnostics;
using MultiTypeParameterGenerator.Analysis.Factories.Collections;
using MultiTypeParameterGenerator.Common.Factories.Entities;
using MultiTypeParameterGenerator.Generation.Factories.Entities;
using Pure.DI;

namespace MultiTypeParameterGenerator;

internal sealed partial class Composition
{
    internal static Composition Instance { get; } = new();

    // @formatter:off
    [Conditional("DI")]
    private void Setup() => DI.Setup()
        .RootBind<IMethodToOverloadFactory>(nameof(MethodToOverloadFactory)).To<MethodToOverloadFactory>()
        .RootBind<ISourceCodeFileFactory>(nameof(SourceCodeFileFactory)).To<SourceCodeFileFactory>()
        .Bind<ITypeFactory>().To<TypeFactory>()
        .Bind<IMethodSourceCodeFactory>().To<MethodSourceCodeFactory>()
        .Bind<IAcceptedTypeCombinationCollectionFactory>().To<AcceptedTypeCombinationCollectionFactory>()
        .Bind<IParameterCollectionFactory>().To<ParameterCollectionFactory>();
    // @formatter:on   
}