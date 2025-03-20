namespace WebServiceClient.Authenticator;

internal class ApiKeyAuthenticator(string name, string value) : IAuthenticator
{
    public void Authenticate(WebService service, HttpClient client)
    {
        client.DefaultRequestHeaders.Add(name, value);
    }
}
