namespace WebServiceClient.Store;

// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation


[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(KeyStore))]
[JsonSerializable(typeof(KeyItem))]
[JsonSerializable(typeof(List<KeyItem>))]
internal partial class KeyStoreContext : JsonSerializerContext
{ }

public class KeyItem
{
    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("login")]
    public string? Login { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }
}

public class KeyStore
{
    private static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KeyStore", "KeyStore.json");


    public required List<KeyItem> Items { get; init; }

    public static KeyStore? Load()
    {
        using Stream stream = File.OpenRead(path);
        return new KeyStore() { Items = JsonSerializer.Deserialize<List<KeyItem>>(stream, KeyStoreContext.Default.ListKeyItem) ?? [] };
    }

    public void Save()
    {
        using Stream stream = File.Create(path);
        JsonSerializer.Serialize<List<KeyItem>>(stream, Items, KeyStoreContext.Default.ListKeyItem);
    }
}
