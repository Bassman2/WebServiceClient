namespace WebServiceClient.Authenticator;

#if NET8_0_OR_GREATER
public class BearerAuthentication(string token) : IAuthenticator
{
#else
public class BearerAuthentication : IAuthenticator
{
    private readonly string token;

    public BearerAuthentication(string token)
    {
        this.token = token;
    }
#endif

    public void Authenticate(WebService service, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
