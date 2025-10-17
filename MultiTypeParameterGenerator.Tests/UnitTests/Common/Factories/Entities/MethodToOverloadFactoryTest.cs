using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MultiTypeParameterGenerator.Common.Factories.Entities;
using NSubstitute;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Common.Factories.Entities;

public static class MethodToOverloadFactoryTest
{
    public sealed class Create
    {
        private static readonly MethodToOverloadFactory MethodToOverloadFactory = new(new TypeFactory());

        [Fact]
        public void CreatesMethodToOverloadFromMethodSymbol()
        {
            // Arrange
            const string code =
                """
                namespace SomeNamespace
                {
                    public sealed class SomeClass
                    {
                        public void SomeMethod<T>(int x) { }
                    }
                }
                       
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly").AddSyntaxTrees(syntaxTree);

            var classSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "SomeClass");
            var methodSymbol = classSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "SomeMethod");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();
            result.Name.Value.Should().Be("SomeMethod");
            result.GenericTypes.Values.Should().ContainSingle().Which.Name.Value.Should().Be("T");
            result.Parameters.Values.Should().ContainSingle().Which.Type.SourceCode.Value.Should().Be("int");
        }

        [Fact]
        public void HandlesStaticMethod()
        {
            // Arrange
            const string code =
                """
                namespace SomeNamespace
                {
                    public sealed class SomeClass
                    {
                        public static void StaticMethod<T>(string text) { }
                    }
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly").AddSyntaxTrees(syntaxTree);

            var classSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "SomeClass");
            var methodSymbol = classSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "StaticMethod");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();
            result.Name.Value.Should().Be("StaticMethod");
            result.AccessModifiers.Values.Should().Contain(m => m.Value == "static");
            result.MethodToOverloadIsStatic.Should().BeTrue();
        }

        [Fact]
        public void HandlesInterface()
        {
            // Arrange
            const string code =
                """
                namespace SomeNamespace
                {
                    public interface ISomeInterface
                    {
                        void InterfaceMethod<T>(T item);
                    }
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly").AddSyntaxTrees(syntaxTree);

            var interfaceSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "ISomeInterface");
            var methodSymbol = interfaceSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "InterfaceMethod");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();
            result.Name.Value.Should().Be("InterfaceMethod");
            result.ContainingType.Kind!.Value.Should().Be("interface");
        }

        [Fact]
        public void HandlesStruct()
        {
            // Arrange
            const string code =
                """
                namespace SomeNamespace
                {
                    public struct SomeStruct
                    {
                        public void StructMethod<T>(T item) { }
                    }
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly").AddSyntaxTrees(syntaxTree);

            var structSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "SomeStruct");
            var methodSymbol = structSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "StructMethod");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();
            result.Name.Value.Should().Be("StructMethod");
            result.ContainingType.Kind!.Value.Should().Be("struct");
        }

        [Fact]
        public void HandlesRecord()
        {
            // Arrange
            const string code =
                """
                namespace SomeNamespace
                {
                    public record SomeRecord
                    {
                        public void RecordMethod<T>(T item) { }
                    }
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly").AddSyntaxTrees(syntaxTree);

            var recordSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "SomeRecord");
            var methodSymbol = recordSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "RecordMethod");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();
            result.Name.Value.Should().Be("RecordMethod");
            result.ContainingType.Kind!.Value.Should().Be("record");
        }

        [Fact]
        public void HandlesRecordStruct()
        {
            // Arrange
            const string code =
                """
                namespace SomeNamespace
                {
                    public record struct SomeRecordStruct
                    {
                        public void RecordStructMethod<T>(T item) { }
                    }
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly").AddSyntaxTrees(syntaxTree);

            var recordStructSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "SomeRecordStruct");
            var methodSymbol = recordStructSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "RecordStructMethod");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();
            result.Name.Value.Should().Be("RecordStructMethod");
            result.ContainingType.Kind!.Value.Should().Be("record struct");
        }

        [Fact]
        public void ReturnsNull_WhenMethodHasNoContainingType()
        {
            // Arrange
            var methodSymbol = Substitute.For<IMethodSymbol>();
            methodSymbol.ContainingType.Returns((INamedTypeSymbol)null!);

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void HandlesGenericMethodWithConstraints()
        {
            // Arrange
            const string code =
                """
                namespace SomeNamespace
                {
                    public sealed class SomeClass
                    {
                        public void ConstrainedMethod<T, U>() 
                            where T : class, IComparable
                            where U : struct, new()
                        { }
                    }
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly")
                .AddReferences(MetadataReference.CreateFromFile(typeof(IComparable).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            var classSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "SomeClass");
            var methodSymbol = classSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "ConstrainedMethod");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();
            result.GenericTypes.Values.Should().HaveCount(2);

            var tConstraint = result.GenericTypes.Values[0].Constraint;
            tConstraint.Should().NotBeNull();
            tConstraint!.HasReferenceTypeConstraint.Should().BeTrue();
            tConstraint.Types.Values.Should().ContainSingle()
                .Which.Value.Should().Be("IComparable");

            var uConstraint = result.GenericTypes.Values[1].Constraint;
            uConstraint.Should().NotBeNull();
            uConstraint!.HasValueTypeConstraint.Should().BeTrue();
            uConstraint.HasConstructorConstraint.Should().BeTrue();
        }

        [Fact]
        public void HandlesExtensionMethodCase_WhenReadOnlyContainingType()
        {
            // Arrange
            const string code =
                """
                namespace SomeNamespace
                {
                    public readonly struct ReadOnlyStruct
                    {
                        public void ExtensionCandidateMethod<T>(T item) { }
                    }
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly").AddSyntaxTrees(syntaxTree);

            var structSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "ReadOnlyStruct");
            var methodSymbol = structSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "ExtensionCandidateMethod");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();
            result.GenerateExtensionMethod.Should().BeTrue();
            result.ContainingType.Kind.Should().BeNull();
        }

        [Fact]
        public void ReturnsNull_WhenPrivateMethodInReadOnlyType()
        {
            // Arrange
            const string code =
                """
                namespace SomeNamespace
                {
                    public readonly struct ReadOnlyStruct
                    {
                        private void PrivateMethod<T>(T item) { }
                    }
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly").AddSyntaxTrees(syntaxTree);

            var structSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "ReadOnlyStruct");
            var methodSymbol = structSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "PrivateMethod");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void HandlesGenericContainingType()
        {
            // Arrange
            const string code =
                """
                namespace SomeNamespace
                {
                    public sealed class GenericClass<T>
                    {
                        public void MethodInGenericClass<U>(T t, U u) { }
                    }
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly").AddSyntaxTrees(syntaxTree);

            var classSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "GenericClass");
            var methodSymbol = classSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "MethodInGenericClass");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();
            result.ContainingType.GenericTypes.Values.Should().ContainSingle()
                .Which.Name.Value.Should().Be("T");
            result.Parameters.Values.Should().HaveCount(2);
            result.Parameters.Values[0].Type.SourceCode.Value.Should().Be("T");
            result.Parameters.Values[1].Type.SourceCode.Value.Should().Be("U");
        }

        [Fact]
        public void HandlesAccessModifiersAttribute()
        {
            // Arrange
            const string code =
                """
                using System;
                using MultiTypeParameterGenerator;
                using static MultiTypeParameterGenerator.AccessModifier;

                namespace SomeNamespace
                {
                    public sealed class SomeClass
                    {
                        [AccessModifiers(Public)]
                        protected void SomeMethod() { }
                    }
                }

                namespace MultiTypeParameterGenerator
                {
                    public enum AccessModifier
                    {
                        Public,
                        Protected,
                        Internal,
                        ProtectedInternal,
                        PrivateProtected
                    }

                    [AttributeUsage(AttributeTargets.Method)]
                    public sealed class AccessModifiersAttribute(AccessModifier accessModifiers) : Attribute;
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly")
                .AddReferences(MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            var classSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "SomeClass");
            var methodSymbol = classSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "SomeMethod");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();
            result.AccessModifiers.Values.Should().ContainSingle()
                .Which.Value.Should().Be("public");
        }

        [Fact]
        public void HandlesAcceptedTypesAttribute()
        {
            // Arrange
            const string code =
                """
                using System;
                using MultiTypeParameterGenerator;

                namespace SomeNamespace
                {
                    public sealed class GenericClass<T>
                    {
                        protected (T1 value1, T2 value2, T3[] value3) MethodWithAttributes<
                            [AcceptedTypes<long, byte, char>(true, $"{nameof(SomeClass<T>)}.{nameof(SomeClass<T>.TestRecord)}", nameof(TestRecord2))]
                            T1,
                            T2,
                            [AcceptedTypes<string, int, bool>(true, $"{nameof(SomeClass<T>)}.{nameof(SomeClass<T>.TestRecord)}", nameof(TestRecord2))]
                            T3>(T1 value1, T2 value2, T3[] value3) where T2 : class?, new();

                        public record TestRecord2;
                    }
                    
                    public sealed class SomeClass<T>
                    {
                        public record TestRecord;
                    }
                }

                namespace MultiTypeParameterGenerator
                {
                    [AttributeUsage(AttributeTargets.GenericParameter)]
                    public abstract class AcceptedTypesAttribute(
                       bool AsGenericTypes, params string[] AdditionalTypes) : Attribute;

                    public sealed class AcceptedTypesAttribute<T1, T2, T3>(
                        bool AsGenericTypes = false, params string[] AdditionalTypes)
                        : AcceptedTypesAttribute(AsGenericTypes, AdditionalTypes);
                }
                """;

            var syntaxTree = CSharpSyntaxTree.ParseText(code, cancellationToken: TestContext.Current.CancellationToken);
            var compilation = CSharpCompilation.Create("SomeAssembly")
                .AddReferences(MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location))
                .AddSyntaxTrees(syntaxTree);

            var classSymbol = compilation.GlobalNamespace
                .GetNamespaceMembers().Single(m => m.Name == "SomeNamespace")
                .GetTypeMembers().Single(t => t.Name == "GenericClass");
            var methodSymbol = classSymbol.GetMembers().OfType<IMethodSymbol>()
                .Single(m => m.Name == "MethodWithAttributes");

            // Act
            var result = MethodToOverloadFactory.Create(methodSymbol);

            // Assert
            result.Should().NotBeNull();

            var acceptedTypesForAffectedGenericTypes = result.AffectedGenericTypes.Values;
            acceptedTypesForAffectedGenericTypes.Should().HaveCount(2);

            acceptedTypesForAffectedGenericTypes[0].AffectedGenericType.Name.Value.Should().Be("T1");
            var acceptedTypesOfFirstAffectedGenericType = acceptedTypesForAffectedGenericTypes[0].AcceptedTypes.Values;
            acceptedTypesOfFirstAffectedGenericType.Should().HaveCount(5);
            acceptedTypesOfFirstAffectedGenericType[0].Type.SourceCode.Value.Should()
                .Be("long");
            acceptedTypesOfFirstAffectedGenericType[0].UseTypeConstraint.Should().BeFalse();
            acceptedTypesOfFirstAffectedGenericType[1].Type.SourceCode.Value.Should()
                .Be("byte");
            acceptedTypesOfFirstAffectedGenericType[1].UseTypeConstraint.Should().BeFalse();
            acceptedTypesOfFirstAffectedGenericType[2].Type.SourceCode.Value.Should()
                .Be("char");
            acceptedTypesOfFirstAffectedGenericType[2].UseTypeConstraint.Should().BeFalse();
            acceptedTypesOfFirstAffectedGenericType[3].Type.SourceCode.Value.Should()
                .Be("SomeNamespace.SomeClass<T>.TestRecord");
            acceptedTypesOfFirstAffectedGenericType[3].UseTypeConstraint.Should().BeTrue();
            acceptedTypesOfFirstAffectedGenericType[4].Type.SourceCode.Value.Should()
                .Be("TestRecord2");
            acceptedTypesOfFirstAffectedGenericType[4].UseTypeConstraint.Should().BeTrue();

            acceptedTypesForAffectedGenericTypes[1].AffectedGenericType.Name.Value.Should().Be("T3");
            var acceptedTypesOfSecondAffectedGenericType = acceptedTypesForAffectedGenericTypes[1].AcceptedTypes.Values;
            acceptedTypesOfSecondAffectedGenericType.Should().HaveCount(5);
            acceptedTypesOfSecondAffectedGenericType[0].Type.SourceCode.Value
                .Should().Be("string");
            acceptedTypesOfSecondAffectedGenericType[0].UseTypeConstraint.Should().BeFalse();
            acceptedTypesOfSecondAffectedGenericType[1].Type.SourceCode.Value
                .Should().Be("int");
            acceptedTypesOfSecondAffectedGenericType[1].UseTypeConstraint.Should().BeFalse();
            acceptedTypesOfSecondAffectedGenericType[2].Type.SourceCode.Value
                .Should().Be("bool");
            acceptedTypesOfSecondAffectedGenericType[2].UseTypeConstraint.Should().BeFalse();
            acceptedTypesOfSecondAffectedGenericType[3].Type.SourceCode.Value
                .Should().Be("SomeNamespace.SomeClass<T>.TestRecord");
            acceptedTypesOfSecondAffectedGenericType[3].UseTypeConstraint.Should().BeTrue();
            acceptedTypesOfSecondAffectedGenericType[4].Type.SourceCode.Value
                .Should()
                .Be("TestRecord2");
            acceptedTypesOfSecondAffectedGenericType[4].UseTypeConstraint.Should().BeTrue();
        }
    }
}