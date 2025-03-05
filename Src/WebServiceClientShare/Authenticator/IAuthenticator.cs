namespace WebServiceClient.Authenticator;

/// <summary>
/// Defines a method to authenticate a web service client.
/// </summary>
public interface IAuthenticator
{
    /// <summary>
    /// Authenticates the specified web service using the provided HTTP client.
    /// </summary>
    /// <param name="service">The web service to authenticate.</param>
    /// <param name="client">The HTTP client used for authentication.</param>
    void Authenticate(WebService service, HttpClient client);
}
