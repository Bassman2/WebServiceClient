namespace WebServiceClient.Authenticator;

public class ApiKeyAuthenticator(string name, string value) : IAuthenticator
{
    public void Authenticate(WebService service, HttpClient client)
    {
        client.DefaultRequestHeaders.Add(name, value);
    }
}
