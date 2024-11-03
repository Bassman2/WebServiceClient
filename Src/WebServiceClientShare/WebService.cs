namespace WebServiceClient;

public class WebService : IDisposable
{
    protected HttpClient? client;

    protected readonly HttpClientHandler httpClientHandler = new()
    {
        CookieContainer = new System.Net.CookieContainer(),
        UseCookies = true,
        ClientCertificateOptions = ClientCertificateOption.Manual,
        ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
    };

    public WebService(Uri host, IAuthenticator? authenticator = null)
    {
        this.Host = host;
        this.client = new HttpClient(httpClientHandler)
        {
            BaseAddress = host,
            Timeout = new TimeSpan(0, 2, 0)
        };
        authenticator?.Authenticate(client!);
    }

    public void Dispose()
    {
        if (client != null)
        {
            client.Dispose();
            client = null;
        }
        GC.SuppressFinalize(this);
    }

    public Uri Host { get; }

    public bool IsConnected => client != null;

    protected virtual void ErrorHandling(HttpResponseMessage response, string memberName)
    {
        throw new WebServiceException("", response.RequestMessage?.RequestUri, response.StatusCode, response.ReasonPhrase, memberName);
    }
}
