using System.Diagnostics;
using MultiTypeParameterGenerator.Analysis.Factories.Collections;
using MultiTypeParameterGenerator.Common.Attributes;
using MultiTypeParameterGenerator.Common.Factories.Entities;
using MultiTypeParameterGenerator.Generation.Factories.Collections;
using MultiTypeParameterGenerator.Generation.Factories.Entities;
using Pure.DI;
using static Pure.DI.Lifetime;

namespace MultiTypeParameterGenerator;

internal partial class Composition
{
    internal static Composition Instance { get; } = new();

    // @formatter:off
    [Conditional("DI")]
    private void Setup() => DI.Setup()
        .RootBind<IMethodToOverloadFactory>(nameof(MethodToOverloadFactory)).To<MethodToOverloadFactory>()
        .RootBind<ISourceCodeFileCollectionFactory>(nameof(SourceCodeFileCollectionFactory)).To<SourceCodeFileCollectionFactory>()
        .RootBind<IAttributesDefinition>(nameof(AttributesDefinition)).As(Singleton).To<AttributesDefinition>()
        .Bind<IAccessModifiersAttributeDefinitionFactory>().As(Singleton).To<AccessModifiersAttributeDefinitionFactory>()
        .Bind<IAcceptedTypesAttributeDefinitionFactory>().As(Singleton).To<AcceptedTypesAttributeDefinitionFactory>()
        .Bind<IMethodSourceCodeCollectionFactory>().To<MethodSourceCodeCollectionFactory>()
        .Bind<IMethodSourceCodeFactory>().To<MethodSourceCodeFactory>()
        .Bind<IAcceptedTypeCombinationCollectionFactory>().To<AcceptedTypeCombinationCollectionFactory>()
        .Bind<IParameterCollectionFactory>().To<ParameterCollectionFactory>()
        .Bind<ISourceCodeFileFactory>().To<SourceCodeFileFactory>();
    // @formatter:on   
}