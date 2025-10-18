using MultiTypeParameterGenerator.Analysis.Factories.Collections;
using MultiTypeParameterGenerator.Analysis.Models.Collections;
using MultiTypeParameterGenerator.Common.Models.Entities;
using MultiTypeParameterGenerator.Generation.Factories.Entities;
using MultiTypeParameterGenerator.Generation.Models.Entities;
using NSubstitute;

namespace MultiTypeParameterGenerator.Tests.UnitTests.Generation.Factories.Entities;

public static class MethodSourceCodeFactoryTest
{
    public sealed class Create
    {
        [Fact]
        public void GeneratesMethodSourceCodeFromMethodToOverload()
        {
            // Arrange
            var methodToOverload = new MethodToOverload(
                false,
                false,
                true,
                new(new("class"), new(new(new("SomeNamespace"), new("SomeClass"))), new()),
                new(new("internal"), new("static")),
                new NamedType(new(null, new("void"))),
                new("SomeMethod"),
                new(new(new("T1")), new(new("T2"))),
                new(
                    new(new(new("T1")), new(
                        new(new NamedType(new(null, new("bool"))), false),
                        new(new NamedType(new(null, new("SomeRecord")), null, new(), true), true))),
                    new(new(new("T2")), new(
                        new(new NamedType(new(null, new("int")), null, new(), true), false),
                        new(new NamedType(new(null, new("SomeRecord"))), true)))),
                new(
                    new(new NamedType(new(null, new("int"))), new("firstParam")),
                    new(new NamedType(new(null, new("T1"))), new("secondParam")),
                    new(new NamedType(new(null, new("T2"))), new("thirdParam"))));

            var acceptedTypeCombinationCollection =
                new AcceptedTypeCombinationCollection(
                    new(new(new(new("T1")), false, new(new NamedType(new(null, new("bool"))), false, 1)),
                        new(new(new("T2")), false, new(new NamedType(new(null, new("int")), null, new(), true), false, 1))),
                    new(new(new(new("T1")), false, new(new NamedType(new(null, new("bool"))), false, 1)),
                        new(new(new("T2")), false, new(new NamedType(new(null, new("SomeRecord"))), true, 1))),
                    new(new(new(new("T1")), false, new(new NamedType(new(null, new("SomeRecord")), null, new(), true), true, 1)),
                        new(new(new("T2")), false, new(new NamedType(new(null, new("int")), null, new(), true), false, 1))),
                    new(new(new(new("T1")), false, new(new NamedType(new(null, new("SomeRecord")), null, new(), true), true, 1)),
                        new(new(new("T2")), false, new(new NamedType(new(null, new("SomeRecord"))), true, 2))));

            var acceptedTypeCombinationCollectionFactory = Substitute.For<IAcceptedTypeCombinationCollectionFactory>();
            acceptedTypeCombinationCollectionFactory.Create(methodToOverload)
                .Returns(acceptedTypeCombinationCollection);

            var parameterCollectionFactory = Substitute.For<IParameterCollectionFactory>();
            parameterCollectionFactory.Create(methodToOverload, Arg.Any<AcceptedTypeCombination>())
                .Returns(args => new(
                    new(new NamedType(new(null, new("int"))), new("firstParam")),
                    new(new GenericTypeParameter(new(null, new("T1"))), new("secondParam"))
                    {
                        TypeNameForSourceCode =
                            args.Arg<AcceptedTypeCombination>().Values[0].AcceptedType.TypeNameForSourceCode
                    },
                    new(new GenericTypeParameter(new(null, new("T2"))), new("thirdParam"))
                    {
                        TypeNameForSourceCode =
                            args.Arg<AcceptedTypeCombination>().Values[1].AcceptedType.TypeNameForSourceCode
                    }));

            var expectedMethodSourceCode = new MethodSourceCode(
                false,
                false,
                new(new("class"), new(new(new("SomeNamespace"), new("SomeClass"))), new()),
                true,
                new("SomeMethod"),
                new("int, T1, T2"),
                new(new(null, new("int")),
                    new(null, new("T1")),
                    new(null, new("T2")),
                    new(null, new("bool")),
                    new(null, new("SomeRecord")),
                    new(null, new("void"))),
                new("""
                       /// <inheritdoc cref="SomeMethod{T1, T2}(int, T1, T2)" />
                       internal static void SomeMethod(int firstParam, bool secondParam, int? thirdParam) =>
                          SomeMethod<bool, int?>(firstParam, secondParam, thirdParam);

                       /// <inheritdoc cref="SomeMethod{T1, T2}(int, T1, T2)" />
                       internal static void SomeMethod<TSomeRecord>(int firstParam, bool secondParam, TSomeRecord thirdParam)
                          where TSomeRecord : SomeRecord =>
                          SomeMethod<bool, TSomeRecord>(firstParam, secondParam, thirdParam);

                       /// <inheritdoc cref="SomeMethod{T1, T2}(int, T1, T2)" />
                       internal static void SomeMethod<TSomeRecord>(int firstParam, TSomeRecord secondParam, int? thirdParam)
                          where TSomeRecord : SomeRecord? =>
                          SomeMethod<TSomeRecord, int?>(firstParam, secondParam, thirdParam);

                       /// <inheritdoc cref="SomeMethod{T1, T2}(int, T1, T2)" />
                       internal static void SomeMethod_WithSomeRecord_AndSomeRecord<TSomeRecord, TSomeRecord_2>(int firstParam, TSomeRecord secondParam, TSomeRecord_2 thirdParam)
                          where TSomeRecord : SomeRecord?
                          where TSomeRecord_2 : SomeRecord =>
                          SomeMethod<TSomeRecord, TSomeRecord_2>(firstParam, secondParam, thirdParam);
                    """));

            // Act
            var methodSourceCode =
                new MethodSourceCodeFactory(acceptedTypeCombinationCollectionFactory, parameterCollectionFactory)
                    .Create(methodToOverload);

            // Assert
            methodSourceCode.Should().BeEquivalentTo(expectedMethodSourceCode);
        }
    }
}