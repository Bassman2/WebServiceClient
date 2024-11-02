namespace WebServiceClient.Authenticator;

#if NET8_0_OR_GREATER
public class ApiKeyAuthenticator(string name, string value) : IAuthenticator
{
#else
public class ApiKeyAuthenticator : IAuthenticator
{
    private readonly string name;
    private readonly string value;

    public ApiKeyAuthenticator(string name, string value)
    {
        this.name = name;
        this.value = value;
    }
#endif

    public void Authenticate(HttpClient client)
    {
        client.DefaultRequestHeaders.Add(name, value);
    }
}
