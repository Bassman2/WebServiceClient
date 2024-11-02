namespace WebServiceClient.JsonConverters;

//2019-04-24T14:50:17.101Z
// "Sat, 21 Sep 2024 21:56:30 +0100"


// 2024-10-30T09:05:10.881+01:00   <- Artifactory


public class JsonDateTimeConverter : JsonConverter<DateTime?>
{
    private static readonly CultureInfo culture =
        //new("en-US");
        CultureInfo.InvariantCulture;

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? str = reader.GetString();
        //if (str[])
        //str = str?.Insert(str.Length - 2, ":");
        if (str == null) return null;
        if (DateTime.TryParse(str, culture, out DateTime dateTime))
        {
            return dateTime.ToLocalTime();
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        string? str = value?.ToString("ddd, dd MM yyyy hh:mm:ss zzz", culture);
        writer.WriteStringValue(str);
    }
}
