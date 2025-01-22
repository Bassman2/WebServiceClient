namespace WebServiceClient.Store;

// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation


[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true, AllowTrailingCommas = true)]
[JsonSerializable(typeof(Dictionary<string, KeyStore>))]
internal partial class KeyStoreContext : JsonSerializerContext
{ }

public class KeyStore
{
    private static readonly string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KeyStore", "KeyStore.json");

    private static Dictionary<string, KeyStore>? items = null;


    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("verify")]
    public string? Verify { get; set; }

    [JsonPropertyName("authentication")]
    public AuthenticationType Authentication { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("user")]
    public string? User { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("login")]
    public string? Login { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    public static KeyStore? Key(string name)
    {
        if (items == null)
        {
            if (System.IO.File.Exists(path))
            {
                // load existing file
                using System.IO.Stream stream = System.IO.File.OpenRead(path);
                items = JsonSerializer.Deserialize<Dictionary<string, KeyStore>>(stream, KeyStoreContext.Default.DictionaryStringKeyStore);
            }
            else
            {
                // create demo file if not exists
                items = new()
                {
                    { "jira", new KeyStore()
                        {
                            Host = "https://www.atlassian.com/",
                            Verify = "https://www.atlassian.com/",
                            Token = "xxxxxxxx",
                            User = "Max Mustermann",
                            Email = "Max.Mustermann@web.de",
                            Login = "mm",
                            Password = "1234",
                            Comment = "Access to Atlassian JIRA"
                        }
                    },
                    { "github", new KeyStore()
                        {
                            Host = "https://github.com/",
                            Verify = "https://github.com/",
                            Token = "xxxxxxxx",
                            User = "Max Mustermann",
                            Email = "Max.Mustermann@web.de",
                            Login = "mm",
                            Password = "1234",
                            Comment = "Access to Microsoft Github"
                        }
                    }
                };
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path)!);
                using System.IO.Stream stream = System.IO.File.Create(path);
                JsonSerializer.Serialize<Dictionary<string, KeyStore>>(stream, items, KeyStoreContext.Default.DictionaryStringKeyStore);
            }
        }
        return items?.GetValueOrDefault(name);
    }
}
