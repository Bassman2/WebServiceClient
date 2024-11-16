namespace WebServiceClient;

public class WebService : IDisposable
{
    protected internal HttpClient? client;

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
        authenticator?.Authenticate(this, this.client);
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
        string str = response.Content.ReadAsStringAsync().Result;
        throw new WebServiceException(str, response.RequestMessage?.RequestUri, response.StatusCode, response.ReasonPhrase, memberName);
    }

    protected virtual async Task ErrorHandlingAsync(HttpResponseMessage response, string memberName, CancellationToken cancellationToken)
    {
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new WebServiceException(str, response.RequestMessage?.RequestUri, response.StatusCode, response.ReasonPhrase, memberName);
    }

    protected string? GetString(string requestUri, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        return response.Content.ReadAsStringAsync().Result;
    }

    protected async Task<string?> GetStringAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public void Post(string requestUri, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.PostAsync(requestUri, null).Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
    }

    protected async Task PostAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.PostAsync(requestUri, null, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
    }

    public void PostFiles(string requestUri, IEnumerable<KeyValuePair<string, Stream>> files, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        var req = new MultipartFormDataContent();
        req.Headers.Add("X-Atlassian-Token", "nocheck");
        foreach (KeyValuePair<string, Stream> file in files)
        {
            req.Add(new StreamContent(file.Value), "file", file.Key);
        }

        using HttpResponseMessage response = client!.PostAsync(requestUri, req).Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
    }

    protected async Task PostFilesAsync(string requestUri, IEnumerable<KeyValuePair<string, Stream>> files, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        var req = new MultipartFormDataContent();
        req.Headers.Add("X-Atlassian-Token", "nocheck");
        foreach (KeyValuePair<string, Stream> file in files)
        {
            req.Add(new StreamContent(file.Value), "file", file.Key);
        }

        using HttpResponseMessage response = await client!.PostAsync(requestUri, req, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
    }

    protected Stream GetFromStream(string requestUri, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        var stream = new MemoryStream();
        response.Content.CopyToAsync(stream).Wait();
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    protected async Task<Stream> GetFromStreamAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        var stream = new MemoryStream();
        await response.Content.CopyToAsync(stream, cancellationToken);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    protected void Download(Uri requestUri, string filePath, [CallerMemberName] string memberName = "")
    {
        //ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        using var file = File.Create(filePath);
        response.Content.CopyToAsync(file).Wait();
    }

    protected void Download(string requestUri, string filePath, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        using var file = File.Create(filePath);
        response.Content.CopyToAsync(file).Wait();
    }

    protected async Task DownloadAsync(Uri requestUri, string filePath, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        //ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        using var file = File.Create(filePath);
        await response.Content.CopyToAsync(file, cancellationToken);
    }

    protected async Task DownloadAsync(string requestUri, string filePath, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        using var file = File.Create(filePath);
        await response.Content.CopyToAsync(file, cancellationToken);
    }

    protected void Delete(string requestUri, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.DeleteAsync(requestUri).Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
    }

    protected async Task DeleteAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.DeleteAsync(requestUri, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
    }
}
