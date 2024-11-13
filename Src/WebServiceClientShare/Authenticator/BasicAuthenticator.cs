namespace WebServiceClient.Authenticator;


internal class BasicAuthenticator(string name, string login, string password, Encoding? encoding = null) : IAuthenticator
{
    public void Authenticate(WebService service, HttpClient client)
    {
        string header = Convert.ToBase64String((encoding ?? Encoding.UTF8).GetBytes($"{login}:{password}"));
        client.DefaultRequestHeaders.Add(name, header);
    }
}


