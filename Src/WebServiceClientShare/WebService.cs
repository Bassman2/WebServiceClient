namespace WebServiceClient;

/// <summary>
/// Represents an abstract base class for web services, providing common functionality for HTTP operations and authentication.
/// </summary>
public abstract class WebService : IDisposable
{
    /// <summary>
    /// The HTTP client used for making requests.
    /// </summary>
    protected internal HttpClient? client;

    /// <summary>
    /// The URL used to test authentication, can be overridden by derived classes.
    /// </summary>
    protected virtual string? AuthenticationTestUrl => null;

    /// <summary>
    /// The HTTP client handler with custom settings.
    /// </summary>
    protected readonly HttpClientHandler httpClientHandler = new()
    {
        CookieContainer = new System.Net.CookieContainer(),
        UseCookies = true,
        ClientCertificateOptions = ClientCertificateOption.Manual,
        ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="WebService"/> class with the specified host, authenticator, and application name.
    /// </summary>
    /// <param name="host">The base URI of the web service.</param>
    /// <param name="authenticator">The authenticator used to authenticate the web service client.</param>
    /// <param name="appName">The name of the application.</param>
    public WebService(Uri host, IAuthenticator? authenticator = null, string? appName = null)
    {
        this.Host = host;
        this.client = new HttpClient(httpClientHandler)
        {
            BaseAddress = host,
            Timeout = new TimeSpan(0, 2, 0)
        };
        WebServiceException.ThrowIfNotConnected(client);
        client.DefaultRequestHeaders.Add("User-Agent", appName ?? "WebServices");
        InitializeClient(client);
        authenticator?.Authenticate(this, this.client);
        if (AuthenticationTestUrl != null)
        {
            TestAutentication();
        }
    }

    /// <summary>
    /// Allows derived classes to customize the initialization of the <see cref="HttpClient"/> instance.
    /// </summary>
    /// <param name="client">The <see cref="HttpClient"/> to be initialized or configured.</param>
    /// <remarks>
    /// Override this method in a derived class to set additional headers, configure default request options,
    /// or apply other custom settings to the HTTP client before it is used for requests.
    /// </remarks>
    protected virtual void InitializeClient(HttpClient client)
    { }

    /// <summary>
    /// Releases the resources used by the <see cref="WebService"/> class.
    /// </summary>
    public void Dispose()
    {
        if (client != null)
        {
            client.Dispose();
            client = null;
        }
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the base URI of the web service.
    /// </summary>
    public Uri Host { get; }

    /// <summary>
    /// Gets a value indicating whether the web service is connected.
    /// </summary>
    public bool IsConnected => client != null;

    /// <summary>
    /// Checks the HTTP response for errors and handles them if any are found.
    /// </summary>
    /// <param name="response">The HTTP response message to check for errors.</param>
    /// <param name="memberName">The name of the calling member. This is automatically set by the compiler.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// If the response indicates a failure, this method calls <see cref="ErrorHandlingAsync"/> to handle the error.
    /// Override if the error handling needs to be customized.
    /// </remarks>
    protected virtual async Task ErrorCheckAsync(HttpResponseMessage response, string memberName, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
    }

    /// <summary>
    /// Handles errors in HTTP responses.
    /// </summary>
    /// <param name="response">The HTTP response message.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected virtual async Task ErrorHandlingAsync(HttpResponseMessage response, string memberName, CancellationToken cancellationToken)
    {
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        throw new WebServiceException(str, response.RequestMessage?.RequestUri, response.StatusCode, response.ReasonPhrase, memberName);
    }

    /// <summary>
    /// Tests the authentication by sending a request to the <see cref="AuthenticationTestUrl"/>.
    /// </summary>
    /// <remarks>Throws <see cref="System.Security.Authentication.AuthenticationException"/> if authentication fails.</remarks>
    protected void TestAutentication()
    {
        WebServiceException.ThrowIfNotConnected(client);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(AuthenticationTestUrl);

        using HttpResponseMessage response = client.GetAsync(AuthenticationTestUrl).Result;
        if (!response.IsSuccessStatusCode)
        {
            throw new AuthenticationException("Authentication failed");
        }
    }

    #region Get

    /// <summary>
    /// Sends a GET request to the specified URI and returns the response body as a string.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response body as a string.</returns>
    protected async Task<string?> GetStringAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        await ErrorCheckAsync(response, memberName, cancellationToken);
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// Sends a GET request to the specified URI and returns the response body as a stream.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the response body as a stream.</returns>
    protected async Task<System.IO.Stream> GetFromStreamAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        await ErrorCheckAsync(response, memberName, cancellationToken);
        var stream = new System.IO.MemoryStream();
        await response.Content.CopyToAsync(stream, cancellationToken);
        stream.Seek(0, System.IO.SeekOrigin.Begin);
        return stream;
    }

    /// <summary>
    /// Sends a GET request to the specified URI and returns a value indicating whether the resource was found.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the resource was found.</returns>
    protected async Task<bool> FoundAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
        await ErrorCheckAsync(response, memberName, cancellationToken);
        return true;
    }

    /// <summary>
    /// Sends a GET request to the specified URI and downloads the response body to the specified file path.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="filePath">The file path to save the response body.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task DownloadAsync(Uri requestUri, string filePath, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        //ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        await ErrorCheckAsync(response, memberName, cancellationToken);
        using var file = System.IO.File.Create(filePath);
        await response.Content.CopyToAsync(file, cancellationToken);
    }

    /// <summary>
    /// Sends a GET request to the specified URI and downloads the response body to the specified file path.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="filePath">The file path to save the response body.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task DownloadAsync(string requestUri, string filePath, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        await ErrorCheckAsync(response, memberName, cancellationToken);
        using var file = System.IO.File.Create(filePath);
        await response.Content.CopyToAsync(file, cancellationToken);
    }

    /// <summary>
    /// Sends a GET request to the specified URI, follows the response location if present, and downloads the response body to the specified file path.
    /// </summary>
    /// <param name="requestUri">The request URI as a string.</param>
    /// <param name="filePath">The file path to save the response body.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <param name="memberName">The name of the calling member. This is automatically set by the compiler.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task DownloadLocationAsync(string requestUri, string filePath, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
        => await DownloadLocationAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), filePath, cancellationToken, memberName);

    /// <summary>
    /// Sends a GET request to the specified URI, follows the response location if present, and downloads the response body to the specified file path.
    /// </summary>
    /// <param name="requestUri">The request URI as a <see cref="Uri"/>.</param>
    /// <param name="filePath">The file path to save the response body.</param>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <param name="memberName">The name of the calling member. This is automatically set by the compiler.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task DownloadLocationAsync(Uri requestUri, string filePath, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage resp = await client.GetAsync(requestUri, cancellationToken);
        Uri? reqUri = resp.RequestMessage?.RequestUri;

        Uri? location = resp.Headers.Location;


        using HttpResponseMessage response = await client.GetAsync(reqUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        await ErrorCheckAsync(response, memberName, cancellationToken);
        using var file = System.IO.File.Create(filePath);
        await response.Content.CopyToAsync(file, cancellationToken);
        //file.Flush();
        //file.Close();
    }

    //{
    //    //ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
    //    WebServiceException.ThrowIfNotConnected(client);

    //    using HttpResponseMessage resp = await client.GetAsync(requestUri, cancellationToken);
    //    Uri? reqUri = resp.RequestMessage?.RequestUri;


    //    using HttpResponseMessage response = await client.GetAsync(reqUri, cancellationToken);
    //    string str = await response.Content.ReadAsStringAsync(cancellationToken);
    //    await ErrorCheckAsync(response, memberName, cancellationToken);
    //    using var file = System.IO.File.Create(filePath);
    //    await response.Content.CopyToAsync(file, cancellationToken);
    //}


    #endregion

    #region Put

    /// <summary>
    /// Sends a PUT request to the specified URI with the provided content.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="content">The HTTP content to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.PutAsync(requestUri, content, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);
    }

    /// <summary>
    /// Sends a PUT request to the specified URI with the provided files.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="files">The files to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task PutFilesAsync(string requestUri, IEnumerable<KeyValuePair<string, System.IO.Stream>> files, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        var req = new MultipartFormDataContent();
        req.Headers.Add("X-Atlassian-Token", "nocheck");
        foreach (KeyValuePair<string, System.IO.Stream> file in files)
        {
            req.Add(new StreamContent(file.Value), "file", file.Key);
        }

        using HttpResponseMessage response = await client.PutAsync(requestUri, req, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);
    }

    #endregion

    #region Post

    /// <summary>
    /// Sends a POST request to the specified URI.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task PostAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.PostAsync(requestUri, null, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        await ErrorCheckAsync(response, memberName, cancellationToken);
    }

    /// <summary>
    /// Sends a POST request to the specified URI with the provided files.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="files">The files to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task PostFilesAsync(string requestUri, IEnumerable<KeyValuePair<string, System.IO.Stream>> files, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        var req = new MultipartFormDataContent();
        req.Headers.Add("X-Atlassian-Token", "nocheck");
        foreach (KeyValuePair<string, System.IO.Stream> file in files)
        {
            req.Add(new StreamContent(file.Value), "file", file.Key);
        }

        using HttpResponseMessage response = await client.PostAsync(requestUri, req, cancellationToken);
        await ErrorCheckAsync(response, memberName, cancellationToken);
    }

    #endregion

    #region Delete

    /// <summary>
    /// Sends a DELETE request to the specified URI.
    /// </summary>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task DeleteAsync(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.DeleteAsync(requestUri, cancellationToken);
        await ErrorCheckAsync(response, memberName, cancellationToken);
    }

    #endregion

    #region Url Helper

    // Nothing : ("Name", null)     => ""
    // Command : ("Name", "")       => "Name"
    // Empty   : ("Name=", " ")      => "Name="    
    // Value   : ("Name", value)    => "Name=Value"
    // Bool    : ("Name", bool)     =>  "Name={true/fals}"  

    private string? Escape(string? str)
    {
        //return Uri.EscapeDataString(str);
        return str?.
            Replace(' ', '+').
            Replace("&", "%26").
            Replace("/", "%2F").
            Replace("=", "%3D").
            Replace("?", "%3F").
            Replace("@", "%40").
            Replace("[", "%5B").
            Replace("]", "%5D");
    }

    /// <summary>
    /// Creates a query string entry from the specified name and value.
    /// </summary>
    /// <param name="entry">The name and value pair.</param>
    /// <returns>The query string entry.</returns>
    protected virtual string QueryEntry((string Name, object? Value) entry)
    {
        string? typeName = entry.Value?.GetType().Name;
        return typeName switch
        {
            "Boolean" => $"{Escape(entry.Name)}={entry.Value?.ToString()?.ToLower()}".TrimEnd('='),
            "String"  => $"{Escape(entry.Name)}={Escape((string?)entry.Value)}",
            _ =>         $"{Escape(entry.Name)}={entry.Value}".TrimEnd('=').Trim(' ') 

        //// used for Confluence Expands
        //case "Enum": return $"{entry.Name}={((Enum)(entry.Value!)).ToString()?.Replace(" ", "").Replace('_', '.').ToLower()}";
        };
    }

   

    /// <summary>
    /// Combines the specified name and value pairs into a query string.
    /// </summary>
    /// <param name="values">The name and value pairs.</param>
    /// <returns>The combined query string.</returns>
    private string CombineQuery(params (string Name, object? Value)[] values)
    {
        //string str = values.
        //    Where(t => t.Value != null).            
        //    Select(t => QueryEntry(t)).
        //    Aggregate("", (a, b) => $"{a}&{b}").Trim('&');
        string str = string.Join('&', values.Where(t => t.Value != null).Select(t => QueryEntry(t)).Where(t => !string.IsNullOrWhiteSpace(t)));
        return str;
    }

    /// <summary>
    /// Combines the specified URL parts into a single URL.
    /// </summary>
    /// <param name="urlParts">The URL parts.</param>
    /// <returns>The combined URL.</returns>
    private static string CombineInt(params string[] urlParts)
    {
        string str = string.Join('/', urlParts.Select(p => p.Trim('/')));
        return str;
    }

    /// <summary>
    /// Combines the specified URL parts into a single URL.
    /// </summary>
    /// <param name="urlParts">The URL parts.</param>
    /// <returns>The combined URL.</returns>
    public static string CombineUrl(params string[] urlParts)
    {
        string str = CombineInt(urlParts); 
        return str;
    }

    /// <summary>
    /// Combines the specified URL part and name-value pairs into a single URL.
    /// </summary>
    /// <param name="urlPartA">The URL part.</param>
    /// <param name="values">The name and value pairs.</param>
    /// <returns>The combined URL.</returns>
    public string CombineUrl(string urlPartA, params (string Name, object? Value)[] values)
    {
        string par = CombineQuery(values);
        string url = CombineInt(urlPartA);
        string str = url.Contains('?') ? $"{url}&{par}".TrimEnd('&') : $"{url}?{par}".TrimEnd('?');
        return str;
    }

    /// <summary>
    /// Combines the specified URL parts and name-value pairs into a single URL.
    /// </summary>
    /// <param name="urlPartA">The first URL part.</param>
    /// <param name="urlPartB">The second URL part.</param>
    /// <param name="values">The name and value pairs.</param>
    /// <returns>The combined URL.</returns>
    public string CombineUrl(string urlPartA, string urlPartB, params (string Name, object? Value)[] values)
    {
        string par = CombineQuery(values);
        string url = CombineInt(urlPartA, urlPartB);
        string str = $"{url}?{par}".TrimEnd('?');
        return str;
    }

    /// <summary>
    /// Combines the specified URL parts and name-value pairs into a single URL.
    /// </summary>
    /// <param name="urlPartA">The first URL part.</param>
    /// <param name="urlPartB">The second URL part.</param>
    /// <param name="urlPartC">The third URL part.</param>
    /// <param name="values">The name and value pairs.</param>
    /// <returns>The combined URL.</returns>
    public string CombineUrl(string urlPartA, string urlPartB, string urlPartC, params (string Name, object? Value)[] values)
    {
        string par = CombineQuery(values);
        string url = CombineInt(urlPartA, urlPartB, urlPartC);
        string str = $"{url}?{par}".TrimEnd('?');
        return str;
    }

    /// <summary>
    /// Combines the specified URL parts and name-value pairs into a single URL.
    /// </summary>
    /// <param name="urlPartA">The first URL part.</param>
    /// <param name="urlPartB">The second URL part.</param>
    /// <param name="urlPartC">The third URL part.</param>
    /// <param name="urlPartD">The fourth URL part.</param>
    /// <param name="values">The name and value pairs.</param>
    /// <returns>The combined URL.</returns>
    public string CombineUrl(string urlPartA, string urlPartB, string urlPartC, string urlPartD, params (string Name, object? Value)[] values)
    {
        string par = CombineQuery(values);
        string url = CombineInt(urlPartA, urlPartB, urlPartC, urlPartD);
        string str = $"{url}?{par}".TrimEnd('?');
        return str;
    }

    /// <summary>
    /// Combines the specified URL parts and name-value pairs into a single URL.
    /// </summary>
    /// <param name="urlPartA">The first URL part.</param>
    /// <param name="urlPartB">The second URL part.</param>
    /// <param name="urlPartC">The third URL part.</param>
    /// <param name="urlPartD">The fourth URL part.</param>
    /// <param name="urlPartE">The fifth URL part.</param>
    /// <param name="values">The name and value pairs.</param>
    /// <returns>The combined URL.</returns>
    public string CombineUrl(string urlPartA, string urlPartB, string urlPartC, string urlPartD, string urlPartE, params (string Name, object? Value)[] values)
    {
        string par = CombineQuery(values);
        string url = CombineInt(urlPartA, urlPartB, urlPartC, urlPartD, urlPartE);
        string str = $"{url}?{par}".TrimEnd('?');
        return str;
    }

    #endregion
}
