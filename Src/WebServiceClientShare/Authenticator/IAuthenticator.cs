namespace WebServiceClient.Authenticator;

public interface IAuthenticator
{
    void Authenticate(HttpClient client);
}
