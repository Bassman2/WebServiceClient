namespace WebServiceClient.Authenticator;

internal class BasicAuthenticator(string login, string password) : IAuthenticator
{
    public void Authenticate(WebService service, HttpClient client)
    {
        string header = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"));
        client.DefaultRequestHeaders.Add("Authorization", "Basic " + header);
    }
}


