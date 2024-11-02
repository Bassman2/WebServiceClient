namespace WebServiceClient.Authenticator;

public class ApiKeyAuthenticator(string name, string value) : IAuthenticator
{
    public void Authenticate(HttpClient client)
    {
        client.DefaultRequestHeaders.Add(name, value);
    }
}
