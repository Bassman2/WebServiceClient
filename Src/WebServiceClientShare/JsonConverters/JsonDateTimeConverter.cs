namespace WebServiceClient.JsonConverters;

//2019-04-24T14:50:17.101Z
// "Sat, 21 Sep 2024 21:56:30 +0100"


// 2024-10-30T09:05:10.881+01:00   <- Artifactory


public class JsonDateTimeConverter : JsonConverter<DateTime?>
{
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

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        string? str = value?.ToString("ddd, dd MM yyyy hh:mm:ss zzz", CultureInfo.InvariantCulture);
        writer.WriteStringValue(str);
    }
}
