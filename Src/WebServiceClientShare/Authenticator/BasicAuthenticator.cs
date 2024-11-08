namespace WebServiceClient.Authenticator;

#if NET8_0_OR_GREATER
internal class BasicAuthenticator(string name, string login, string password, Encoding? encoding = null) : IAuthenticator
{
#else
internal class BasicAuthenticator : IAuthenticator
{
    private readonly string name;
    private readonly string login;
    private readonly string password;
    private readonly Encoding? encoding;

    public BasicAuthenticator(string name, string login, string password, Encoding? encoding = null)
    {
        this.name = name;
        this.login = login;
        this.password = password;
        this.encoding = encoding;
    }
#endif

    public void Authenticate(WebService service, HttpClient client)
    {
        string header = Convert.ToBase64String((encoding ?? Encoding.UTF8).GetBytes($"{login}:{password}"));
        client.DefaultRequestHeaders.Add(name, header);
    }
}


