namespace WebServiceClient.JsonConverters;

//2019-04-24T14:50:17.101Z
// "Sat, 21 Sep 2024 21:56:30 +0100"


// 2024-10-30T09:05:10.881+01:00   <- Artifactory


/// <summary>
/// Converts DateTime? objects to and from JSON, if ":" is missing in time zone.
/// </summary>
public class JsonDateTimeConverter : JsonConverter<DateTime?>
{
    /// <summary>
    /// Reads and converts the JSON to a DateTime? object.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="typeToConvert">The type to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The converted DateTime? object.</returns>
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? str = reader.GetString();
        ////if (str[])
        ////str = str?.Insert(str.Length - 2, ":");
        //if (str == null) return null;

        if (DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
        {
            return dateTime.ToLocalTime();
        }

        return null;
    }

    /// <summary>
    /// Writes a DateTime? object as a JSON string.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        string? str = value?.ToString("ddd, dd MM yyyy hh:mm:ss zzz", CultureInfo.InvariantCulture);
        writer.WriteStringValue(str);
    }
}
