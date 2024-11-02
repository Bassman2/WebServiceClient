namespace WebServiceClient.Authenticator;

public class BearerAuthentication(string token) : IAuthenticator
{
    public void Authenticate(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
