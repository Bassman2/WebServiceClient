namespace WebServiceClient.Authenticator;

internal class MultiAuthenticator(params IAuthenticator[] authenticators) : IAuthenticator
{
    public void Authenticate(WebService service, HttpClient client)
    {
        foreach (IAuthenticator authenticator in authenticators)
        {
            authenticator.Authenticate(service, client);
        }
    }
}
