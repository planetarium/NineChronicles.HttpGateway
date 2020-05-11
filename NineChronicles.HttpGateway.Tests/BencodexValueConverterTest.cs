namespace NineChronicles.HttpGateway.Tests
{
    using System.Collections.Generic;
    using System.Numerics;
    using System.Text.Json;
    using Bencodex.Types;
    using Xunit;

    public class BencodexValueConverterTest
    {
        public static IEnumerable<object[]> Data =>
            new[]
            {
                // Cases for `Bencodex.Types.Text`
                new object[]
                {
                    (Text)"foo", "{\"value\":\"foo\"}",
                },
                new object[]
                {
                    (Text)"bar", "{\"value\":\"bar\"}",
                },
                new object[]
                {
                    (Text)string.Empty, "{\"value\":\"\"}",
                },

                // Cases for `Bencodex.Types.Bool`
                new object[]
                {
                    (Bencodex.Types.Boolean)true, "{\"value\":true}",
                },
                new object[]
                {
                    (Bencodex.Types.Boolean)false, "{\"value\":false}",
                },

                // Cases for `Bencodex.Types.Integer`
                new object[]
                {
                    (Bencodex.Types.Integer)BigInteger.Zero, "{\"value\":0}",
                },
                new object[]
                {
                    (Bencodex.Types.Integer)long.MinValue, $"{{\"value\":{long.MinValue}}}",
                },
                new object[]
                {
                    (Bencodex.Types.Integer)long.MaxValue, $"{{\"value\":{long.MaxValue}}}",
                },

                // Cases for `Bencodex.Types.List`
                new object[]
                {
                    new List(new IValue[]
                    {
                        (Text)"foo", (Bencodex.Types.Boolean)true, (Bencodex.Types.Integer)0,
                    }),
                    "{\"value\":[\"foo\",true,0]}",
                },

                // Cases for `Bencodex.Types.Dictionary` and recursive presentation
                new object[]
                {
                    Dictionary.Empty.Add("foo", "bar").Add("child", Dictionary.Empty.Add("baz", "qux")),
                    "{\"value\":{\"child\":{\"baz\":\"qux\"},\"foo\":\"bar\"}}",
                },
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void Convert(IValue value, string expected)
        {
            var serialized = JsonSerializer.Serialize(value, new JsonSerializerOptions
            {
                Converters =
                {
                    new BencodexValueConverter(),
                },
            });
            Assert.Equal(expected, serialized);
        }
    }
}