namespace WebServiceClient.Authenticator;

internal class Basic2Authenticator(string token) : IAuthenticator
{
    public void Authenticate(WebService service, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", token);
    }
}
