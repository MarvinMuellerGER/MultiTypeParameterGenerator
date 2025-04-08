using JetBrains.Annotations;
using static MultiTypeParameterGenerator.AccessModifier;

namespace MultiTypeParameterGenerator.Tests.IntegrationTests;

public class Generator
{
    public class ForMethodWithAcceptedTypesAttributeInInterface
    {
        [Fact]
        public void OverloadsOfAllPossibleCombinationsShouldBeGenerated()
        {
            // Arrange
#pragma warning disable CA1859
            ISomeClass<bool> someClass = new SomeClass<bool>();
#pragma warning restore CA1859
            // ReSharper disable SuggestVarOrType_BuiltInTypes
            object objectValue = new();
            const long longValue = 0;
            const byte byteValue = 0;
            const char charValue = ' ';
            const string stringValue = "";
            const int intValue = 0;
            const bool boolValue = false;
            // ReSharper restore SuggestVarOrType_BuiltInTypes
            var someRecord = new SomeClass<bool>.SomeRecord();
            var someRecord2 = new ISomeClass<bool>.SomeRecord();
            var someOtherRecord = new SomeOtherClass.SomeOtherRecord();

            // Act & Assert
            someClass.SomeMethod(longValue, objectValue, [stringValue])
                .Should().BeEquivalentTo((longValue, objectValue, new[] { stringValue }));

            someClass.SomeMethod(longValue, objectValue, [intValue])
                .Should().BeEquivalentTo((longValue, objectValue, new[] { intValue }));

            someClass.SomeMethod(longValue, objectValue, [boolValue])
                .Should().BeEquivalentTo((longValue, objectValue, new[] { boolValue }));

            someClass.SomeMethod(longValue, objectValue, [someRecord])
                .Should().BeEquivalentTo((longValue, objectValue, new[] { someRecord }));

            someClass.SomeMethod_WithSomeRecord_2(longValue, objectValue, [someRecord2])
                .Should().BeEquivalentTo((longValue, objectValue, new[] { someRecord2 }));

            someClass.SomeMethod(byteValue, objectValue, [stringValue])
                .Should().BeEquivalentTo((byteValue, objectValue, new[] { stringValue }));

            someClass.SomeMethod(byteValue, objectValue, [intValue])
                .Should().BeEquivalentTo((byteValue, objectValue, new[] { intValue }));

            someClass.SomeMethod(byteValue, objectValue, [boolValue])
                .Should().BeEquivalentTo((byteValue, objectValue, new[] { boolValue }));

            someClass.SomeMethod(byteValue, objectValue, [someRecord])
                .Should().BeEquivalentTo((byteValue, objectValue, new[] { someRecord }));

            someClass.SomeMethod_WithSomeRecord_2(byteValue, objectValue, [someRecord2])
                .Should().BeEquivalentTo((byteValue, objectValue, new[] { someRecord2 }));

            someClass.SomeMethod(charValue, objectValue, [stringValue])
                .Should().BeEquivalentTo((charValue, objectValue, new[] { stringValue }));

            someClass.SomeMethod(charValue, objectValue, [intValue])
                .Should().BeEquivalentTo((charValue, objectValue, new[] { intValue }));

            someClass.SomeMethod(charValue, objectValue, [boolValue])
                .Should().BeEquivalentTo((charValue, objectValue, new[] { boolValue }));

            someClass.SomeMethod(charValue, objectValue, [someRecord])
                .Should().BeEquivalentTo((charValue, objectValue, new[] { someRecord }));

            someClass.SomeMethod_WithSomeRecord_2(charValue, objectValue, [someRecord2])
                .Should().BeEquivalentTo((charValue, objectValue, new[] { someRecord2 }));

            someClass.SomeMethod(someRecord, objectValue, [stringValue])
                .Should().BeEquivalentTo((testRecord: someRecord, objectValue, new[] { stringValue }));

            someClass.SomeMethod(someRecord, objectValue, [intValue])
                .Should().BeEquivalentTo((testRecord: someRecord, objectValue, new[] { intValue }));

            someClass.SomeMethod(someRecord, objectValue, [boolValue])
                .Should().BeEquivalentTo((testRecord: someRecord, objectValue, new[] { boolValue }));

            someClass.SomeMethod_WithSomeRecord_AndSomeRecord(someRecord, objectValue, [someRecord])
                .Should().BeEquivalentTo((testRecord: someRecord, objectValue, new[] { someRecord }));

            someClass.SomeMethod_WithSomeRecord_AndSomeRecord_2(someRecord, objectValue, [someRecord2])
                .Should().BeEquivalentTo((testRecord: someRecord, objectValue, new[] { someRecord2 }));

            someClass.SomeMethod_WithSomeRecord_2(someRecord2, objectValue, [stringValue])
                .Should().BeEquivalentTo((testRecord2: someRecord2, objectValue, new[] { stringValue }));

            someClass.SomeMethod_WithSomeRecord_2(someRecord2, objectValue, [intValue])
                .Should().BeEquivalentTo((testRecord2: someRecord2, objectValue, new[] { intValue }));

            someClass.SomeMethod_WithSomeRecord_2(someRecord2, objectValue, [boolValue])
                .Should().BeEquivalentTo((testRecord2: someRecord2, objectValue, new[] { boolValue }));

            someClass.SomeMethod_WithSomeRecord_2_AndSomeRecord(someRecord2, objectValue, [someRecord])
                .Should().BeEquivalentTo((testRecord2: someRecord2, objectValue, new[] { someRecord }));

            someClass.SomeMethod_WithSomeRecord_2_AndSomeRecord_2(someRecord2, objectValue, [someRecord2])
                .Should().BeEquivalentTo((testRecord2: someRecord2, objectValue, new[] { someRecord2 }));

            SomeOtherClass.SomeMethod(boolValue).Should().Be(boolValue);
            SomeOtherClass.SomeMethod(someOtherRecord).Should().Be(someOtherRecord);
        }
    }
}

public partial interface ISomeClass<T> where T : struct
{
    private const string TestRecordName = $"{nameof(SomeClass<>)}.{nameof(SomeClass<>.SomeRecord)}?";

    // @formatter:off
    [AccessModifiers(Public)]
    protected (T1 value1, T2 value2, T3[] value3) SomeMethod<
        [AcceptedTypes<long, byte, char>(true, TestRecordName, nameof(SomeRecord))] T1,
        T2, [AcceptedTypes<string, int, bool>(true, TestRecordName, nameof(SomeRecord))]
        T3>
        (T1 value1, T2 value2, T3[] value3) where T2 : class?, new();
    // @formatter:on

    public interface ISomeRecord;

    public record SomeRecord;
}

public class SomeClass<T> : ISomeClass<T> where T : struct
{
    public (T1 value1, T2 value2, T3[] value3) SomeMethod<T1, T2, T3>(T1 value1, T2 value2, T3[] value3)
        where T2 : class?, new() => (value1, value2, value3);

    public record SomeRecord : ISomeClass<T>.ISomeRecord;
}

[UsedImplicitly]
public static partial class SomeOtherClass
{
    [AccessModifiers(Internal)]
    private static T SomeMethod<[AcceptedTypes<bool, SomeOtherRecord>] T>(T value) => value;

    public record SomeOtherRecord;
}