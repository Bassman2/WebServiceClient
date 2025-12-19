namespace WebServiceClient.Authenticator;

internal class TokenAuthenticator(string name, string value) : IAuthenticator
{
    public void Authenticate(WebService service, HttpClient client)
    {
        client.DefaultRequestHeaders.Add(name, value);
        //sender.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(name, value);

    }
}
