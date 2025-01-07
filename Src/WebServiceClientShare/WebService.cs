namespace WebServiceClient;

public abstract class WebService : IDisposable
{
    protected internal HttpClient? client;
    protected virtual string? AuthenticationTestUrl => null;

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
        if (AuthenticationTestUrl != null)
        {
            TestAutentication();
        }
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

    protected virtual async Task ErrorHandlingAsync(HttpResponseMessage response, string memberName, CancellationToken cancellationToken)
    {
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new WebServiceException(str, response.RequestMessage?.RequestUri, response.StatusCode, response.ReasonPhrase, memberName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Throw System.Security.Authentication.AuthenticationException if authentication failed.</remarks>
    protected void TestAutentication()
    {
        WebServiceException.ThrowIfNullOrNotConnected(this);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(AuthenticationTestUrl);

        using HttpResponseMessage response = client!.GetAsync(AuthenticationTestUrl).Result;
        if (!response.IsSuccessStatusCode)
        {
            throw new AuthenticationException("Authentication failed");
        }
    }

    #region Get

    protected async Task<string?> GetStringAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    protected async Task<System.IO.Stream> GetFromStreamAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
        var stream = new System.IO.MemoryStream();
        await response.Content.CopyToAsync(stream, cancellationToken);
        stream.Seek(0, System.IO.SeekOrigin.Begin);
        return stream;
    }

    protected async Task<bool> FoundAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
        return true;
    }

    protected async Task DownloadAsync(Uri requestUri, string filePath, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        //ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
        using var file = System.IO.File.Create(filePath);
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
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
        using var file = System.IO.File.Create(filePath);
        await response.Content.CopyToAsync(file, cancellationToken);
    }

    #endregion

    #region Put

    protected async Task PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.PutAsync(requestUri, content, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
    }

    protected async Task PutFilesAsync(string requestUri, IEnumerable<KeyValuePair<string, System.IO.Stream>> files, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        var req = new MultipartFormDataContent();
        req.Headers.Add("X-Atlassian-Token", "nocheck");
        foreach (KeyValuePair<string, System.IO.Stream> file in files)
        {
            req.Add(new StreamContent(file.Value), "file", file.Key);
        }

        using HttpResponseMessage response = await client!.PutAsync(requestUri, req, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
    }

    #endregion

    #region Post

    protected async Task PostAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.PostAsync(requestUri, null, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
    }

    protected async Task PostFilesAsync(string requestUri, IEnumerable<KeyValuePair<string, System.IO.Stream>> files, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        var req = new MultipartFormDataContent();
        req.Headers.Add("X-Atlassian-Token", "nocheck");
        foreach (KeyValuePair<string, System.IO.Stream> file in files)
        {
            req.Add(new StreamContent(file.Value), "file", file.Key);
        }

        using HttpResponseMessage response = await client!.PostAsync(requestUri, req, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
    }

    #endregion

    #region Delete

    protected async Task DeleteAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.DeleteAsync(requestUri, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
    }

    #endregion

    #region Url Helper

    // Nothing : ("Name", null)     => ""
    // Command : ("Name", "")       => "Name"
    // Empty   : ("Name=", " ")      => "Name="    
    // Value   : ("Name", value)    => "Name=Value"
    // Bool    : ("Name", bool)     =>  "Name={true/fals}"  

    private static string QueryEntry((string Name, object? Value) entry)
    {
        string? typeName = entry.Value?.GetType().Name; 
        switch (typeName)
        {
        case "Boolean": return $"{entry.Name}={entry.Value?.ToString()?.ToLower()}".TrimEnd('=');
        }
        //Select(static t => (t.Name, Value: t.Value!.GetType().Name switch
        //    {
        //        "Bool" => t.Value.ToString()?.ToLower(),
        //        "String" => t.Value.ToString(),
        //        _ => throw new Exception(t.Value.GetType().Name)
        //    }
        //)).

        return $"{entry.Name}={entry.Value}".TrimEnd('=').Trim(' ');
    }
    private static string CombineQuery(params (string Name, object? Value)[] values)
    {
        //string str = values.
        //    Where(t => t.Value != null).            
        //    Select(t => QueryEntry(t)).
        //    Aggregate("", (a, b) => $"{a}&{b}").Trim('&');
        string str = string.Join('&', values.Where(t => t.Value != null).Select(t => QueryEntry(t)));
        return str;
    }
    private static string CombineInt(params string[] urlParts)
    {
        string str = string.Join('/', urlParts.Select(p => p.Trim('/')));
        return str;
    }

    public static string CombineUrl(params string[] urlParts)
    {
        string str = CombineInt(urlParts); 
        return str;
    }

    public static string CombineUrl(string urlPartA, params (string Name, object? Value)[] values)
    {
        string par = CombineQuery(values);
        string url = CombineInt(urlPartA);
        string str = $"/{url}?{par}".TrimEnd('?');
        return str;
    }

    public static string CombineUrl(string urlPartA, string urlPartB, params (string Name, object? Value)[] values)
    {
        string par = CombineQuery(values);
        string url = CombineInt(urlPartA, urlPartB);
        string str = $"/{url}?{par}".TrimEnd('?');
        return str;
    }

    public static string CombineUrl(string urlPartA, string urlPartB, string urlPartC, params (string Name, object? Value)[] values)
    {
        string par = CombineQuery(values);
        string url = CombineInt(urlPartA, urlPartB, urlPartC);
        string str = $"/{url}?{par}".TrimEnd('?');
        return str;
    }

    public static string CombineUrl(string urlPartA, string urlPartB, string urlPartC, string urlPartD, params (string Name, object? Value)[] values)
    {
        string par = CombineQuery(values);
        string url = CombineInt(urlPartA, urlPartB, urlPartC, urlPartD);
        string str = $"/{url}?{par}".TrimEnd('?');
        return str;
    }

    public static string CombineUrl(string urlPartA, string urlPartB, string urlPartC, string urlPartD, string urlPartE, params (string Name, object? Value)[] values)
    {
        string par = CombineQuery(values);
        string url = CombineInt(urlPartA, urlPartB, urlPartC, urlPartD, urlPartE);
        string str = $"/{url}?{par}".TrimEnd('?');
        return str;
    }

    #endregion
}
