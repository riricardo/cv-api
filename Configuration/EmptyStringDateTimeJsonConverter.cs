using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace cv_api.Configuration;

public class EmptyStringDateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();

            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }

            return DateTime.Parse(
                value,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind
            );
        }

        if (reader.TokenType == JsonTokenType.Null)
        {
            return default;
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            return default;
        }

        throw new JsonException($"Unable to convert {reader.TokenType} to DateTime.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        DateTime value,
        JsonSerializerOptions options
    )
    {
        writer.WriteStringValue(value);
    }
}
