using Microsoft.CodeAnalysis;
using MultiTypeParameterGenerator.Common.Extensions.Collections;
using MultiTypeParameterGenerator.Common.Extensions.Roslyn;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Common.Utils;

namespace MultiTypeParameterGenerator;

[Generator]
internal class Generator : IIncrementalGenerator
{
    private static readonly IList<IMethodSymbol> ProcessedMethods = new List<IMethodSymbol>();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            GetMethodsToGenerate(context).SelectMany((methodsToOverload, _) =>
                methodsToOverload),
            static (context, source) => Execute(context, source));
    }

    private static IncrementalValuesProvider<IReadOnlyList<MethodToOverload>> GetMethodsToGenerate(
        IncrementalGeneratorInitializationContext context)
    {
        var incrementalValuesProvider = context.SyntaxProvider
            .ForPolyGenericAttributeWithMetadataName(
                typeof(AcceptedTypesAttribute).FullName!,
                static (_, _) => true,
                GetMethodsToGenerate)
            .Where(static m => m is not null)
            .Select(static (m, _) => m!);

        ProcessedMethods.Clear();

        return incrementalValuesProvider;
    }

    private static IReadOnlyList<MethodToOverload> GetMethodsToGenerate(
        GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (context.TargetSymbol is not ITypeParameterSymbol typeParameterSymbol)
            return [];

        return typeParameterSymbol switch
        {
            { DeclaringType: not null } => GetMethodsToGenerate(typeParameterSymbol.DeclaringType),
            { DeclaringMethod: not null } => GetMethodsToGenerate(typeParameterSymbol.DeclaringMethod),
            _ => []
        };
    }

    private static IReadOnlyList<MethodToOverload> GetMethodsToGenerate(INamedTypeSymbol declaringType)
    {
        var typeParameters = declaringType.TypeParameters.Select(typeParameter => typeParameter.Name);
        var methods = declaringType.GetMembers().OfType<IMethodSymbol>().Where(method =>
            method.Parameters.Any(parameter => typeParameters.Contains(parameter.Type.Name)));

        return methods.Select(GetMethodsToGenerate).SelectManyToReadonlyList(m => m);
    }

    private static IReadOnlyList<MethodToOverload> GetMethodsToGenerate(IMethodSymbol method)
    {
        if (ProcessedMethods.Contains(method))
            return [];

        ProcessedMethods.Add(method);

        try
        {
            var methodToOverload = Composition.Instance.MethodToOverloadFactory.Create(method);
            return methodToOverload is null ? [] : [methodToOverload];
        }
        catch (Exception e)
        {
            var stackTrace = e.StackTrace.Replace(Constants.NewLine, Constants.Whitespace);
            throw new($"{e.Message} (StackTrace: {stackTrace})", e);
        }
    }

    private static void Execute(SourceProductionContext context, in MethodToOverload methodToOverload)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        try
        {
            context.AddSource(Composition.Instance.SourceCodeFileFactory.Create(methodToOverload));
        }
        catch (Exception e)
        {
            var stackTrace = e.StackTrace.Replace(Constants.NewLine, Constants.Whitespace);
            throw new($"{e.Message} (StackTrace: {stackTrace})", e);
        }
    }
}