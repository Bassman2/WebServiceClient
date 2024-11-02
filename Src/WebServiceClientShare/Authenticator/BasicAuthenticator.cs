namespace WebServiceClient.Authenticator;

internal class BasicAuthenticator : IAuthenticator
{
    private readonly string header;

    public BasicAuthenticator(string login, string password)
        : this(login, password, Encoding.UTF8)
    {
        
    }
    public BasicAuthenticator(string login, string password, Encoding encoding)
    {
        header = Convert.ToBase64String(encoding.GetBytes($"{login}:{password}"));
    }

    public void Authenticate(HttpClient client)
    {
        throw new NotImplementedException();
    }
}


