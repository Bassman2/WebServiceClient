namespace WebServiceClient.Store;

// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation


[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true, AllowTrailingCommas = true)]
[JsonSerializable(typeof(Dictionary<string, KeyStore>))]
internal partial class KeyStoreContext : JsonSerializerContext
{ }

public class KeyStore
{
    private static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KeyStore", "KeyStore.json");

    private static Dictionary<string, KeyStore>? items = null;


    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("login")]
    public string? Login { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    public static KeyStore? Key(string name)
    {
        if (items == null)
        {
            if (File.Exists(path))
            {
                // load existing file
                using Stream stream = File.OpenRead(path);
                items = JsonSerializer.Deserialize<Dictionary<string, KeyStore>>(stream, KeyStoreContext.Default.DictionaryStringKeyStore);
            }
            else
            {
                // create demo file if not exists
                items = new()
                {
                    { "jira", new KeyStore() { Host = "https://www.atlassian.com/", Login = "mm", Password = "1234", Token = "", Name = "Max Mustermann", Email = "Max.Mustermann@web.de" } },
                    { "github", new KeyStore() { Host = "https://github.com/", Login = "mm", Password = "1234", Token = "", Name = "Max Mustermann", Email = "Max.Mustermann@web.de" } }
                };
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                using Stream stream = File.Create(path);
                JsonSerializer.Serialize<Dictionary<string, KeyStore>>(stream, items, KeyStoreContext.Default.DictionaryStringKeyStore);
            }
        }
        return items?.GetValueOrDefault(name);
    }
}
