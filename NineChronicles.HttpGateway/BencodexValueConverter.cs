namespace NineChronicles.HttpGateway
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Bencodex.Types;
    using Boolean = Bencodex.Types.Boolean;

    public class BencodexValueConverter : JsonConverter<IValue>
    {
        public override IValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IValue value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("value");
            this.WriteInner(writer, value, options);
            writer.WriteEndObject();
        }

        private string ToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        private void WriteInner(Utf8JsonWriter writer, IValue value, JsonSerializerOptions options)
        {
            if (value is Dictionary dictionary)
            {
                writer.WriteStartObject();
                foreach (var pair in dictionary)
                {
                    // Property in json must be string.
                    switch (pair.Key)
                    {
                        case Text text:
                            writer.WritePropertyName(text.Value);
                            break;
                        case Binary binary:
                            writer.WritePropertyName(this.ToHex(binary.Value));
                            break;
                    }

                    this.WriteInner(writer, pair.Value, options);
                }

                writer.WriteEndObject();
            }
            else if (value is List list)
            {
                writer.WriteStartArray();
                foreach (var v in list)
                {
                    this.WriteInner(writer, v, options);
                }

                writer.WriteEndArray();
            }
            else if (value is Text text)
            {
                writer.WriteStringValue(text.Value);
            }
            else if (value is Binary binary)
            {
                writer.WriteStringValue(this.ToHex(binary.Value));
            }
            else if (value is Null)
            {
                writer.WriteNullValue();
            }
            else if (value is Integer integer)
            {
                writer.WriteNumberValue((long)integer);
            }
            else if (value is Boolean boolean)
            {
                writer.WriteBooleanValue(boolean);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
