namespace WebServiceClient.Store;

// https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation


[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true, AllowTrailingCommas = true)]
[JsonSerializable(typeof(Dictionary<string, KeyStore>))]
internal partial class KeyStoreContext : JsonSerializerContext
{ }

/// <summary>
/// Represents a storage for key-value pairs related to web service authentication and configuration.
/// Provides methods to retrieve and initialize key store items from a JSON file.
/// </summary>
public class KeyStore
{
    private static readonly string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "KeyStore", "KeyStore.json");

    private static Dictionary<string, KeyStore>? items = null;

    /// <summary>
    /// Gets or sets the host URL.
    /// </summary>
    [JsonPropertyName("host")]
    public string? Host { get; set; }

    /// <summary>
    /// Gets or sets the URL to verify the host.
    /// </summary>
    [JsonPropertyName("verify")]
    public string? Verify { get; set; }

    /// <summary>
    /// Gets or sets the type of authentication used.
    /// </summary>
    [JsonPropertyName("authentication")]
    public AuthenticationType Authentication { get; set; }

    /// <summary>
    /// Gets or sets the authentication token.
    /// </summary>
    [JsonPropertyName("token")]
    public string? Token { get; set; }

    [JsonPropertyName("tokenexpire")]
    public DateTime? TokenExpire { get; set; }

    /// <summary>
    /// Gets or sets the user name.
    /// </summary>
    [JsonPropertyName("user")]
    public string? User { get; set; }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the login name.
    /// </summary>
    [JsonPropertyName("login")]
    public string? Login { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    /// <summary>
    /// Url to token update page.
    /// </summary>
    [JsonPropertyName("update")]
    public string? Update { get; set; }

    /// <summary>
    /// Retrieves a KeyStore item by name. If the items dictionary is not initialized, it loads from the JSON file or creates a demo file if it doesn't exist.
    /// </summary>
    /// <param name="name">The name of the KeyStore item to retrieve.</param>
    /// <returns>The KeyStore item if found; otherwise, null.</returns>
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
                            TokenExpire = DateTime.Now,
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
                            TokenExpire = DateTime.Now,
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
