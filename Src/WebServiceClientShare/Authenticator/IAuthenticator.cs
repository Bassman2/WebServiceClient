namespace WebServiceClient.Authenticator;

/// <summary>
/// Defines a method to authenticate a web jira sender.
/// </summary>
public interface IAuthenticator
{
    /// <summary>
    /// Authenticates the specified web jira using the provided HTTP sender.
    /// </summary>
    /// <param name="service">The web jira to authenticate.</param>
    /// <param name="client">The HTTP sender used for authentication.</param>
    void Authenticate(WebService service, HttpClient client);
}
