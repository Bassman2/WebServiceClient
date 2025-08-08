namespace WebServiceClient;

/// <summary>
/// Represents an abstract base class for JSON-based web services, providing common functionality for HTTP operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="JsonService"/> class with the specified host, authenticator, application name, and JSON serializer context.
/// </remarks>
/// <param name="host">The base URI of the web service.</param>
/// <param name="authenticator">The authenticator used to authenticate the web service client.</param>
/// <param name="appName">The name of the application.</param>
/// <param name="context">The JSON serializer context.</param>
public abstract class JsonService(Uri host, IAuthenticator? authenticator, string appName, JsonSerializerContext context) : WebService(host, authenticator, appName)
{
    /// <summary>
    /// The JSON serializer context.
    /// </summary>
    protected readonly JsonSerializerContext context = context;

    /// <summary>
    /// Configures the <see cref="HttpClient"/> instance for JSON-based web service requests.
    /// </summary>
    /// <param name="client">The <see cref="HttpClient"/> to be initialized or configured.</param>
    /// <remarks>
    /// Sets the default request headers to accept JSON and other common content types.
    /// Override this method to apply additional client configuration before making requests.
    /// </remarks>
    protected override void InitializeClient(HttpClient client)
    {
    //    client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        // content-type    text/html;charset=UTF-8
        // client.DefaultRequestHeaders.Conmtent
        client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.org.jfrog.artifactory.system.ExportSettings+json");
        base.InitializeClient(client);
    }

    #region Get

    /// <summary>
    /// Sends a GET request to the specified URI and returns the response body deserialized as an object of type <typeparamref name="OUT"/>.
    /// </summary>
    /// <typeparam name="OUT">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<OUT?> GetFromJsonAsync<OUT>(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.GetAsync(requestUri, cancellationToken);

#if DEBUG
        string str = response.Content.ReadAsStringAsync(cancellationToken).Result;
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);


        JsonTypeInfo<OUT> jsonTypeInfoOut = (JsonTypeInfo<OUT>)context.GetTypeInfo(typeof(OUT))!;
        var model = await response.Content.ReadFromJsonAsync<OUT>(jsonTypeInfoOut, cancellationToken);
        return model;
    }

    #endregion

    #region Put

    /// <summary>
    /// Sends a PUT request to the specified URI with the provided object serialized as JSON.
    /// </summary>
    /// <typeparam name="IN">The type of the request object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task PutAsJsonAsync<IN>(string requestUri, IN obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNotConnected(client);

        JsonTypeInfo<IN> jsonTypeInfo = (JsonTypeInfo<IN>)context.GetTypeInfo(typeof(IN))!;

#if DEBUG
        string str = JsonSerializer.Serialize<IN>(obj, jsonTypeInfo);
#endif   

        using HttpResponseMessage response = await client.PutAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);
        await ErrorCheckAsync(response, memberName, cancellationToken);
    }

    /// <summary>
    /// Sends a PUT request to the specified URI with the provided object serialized as JSON and returns the response body deserialized as an object of type <typeparamref name="OUT"/>.
    /// </summary>
    /// <typeparam name="IN">The type of the request object.</typeparam>
    /// <typeparam name="OUT">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<OUT?> PutAsJsonAsync<IN, OUT>(string requestUri, IN obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNotConnected(client);

        JsonTypeInfo<IN> jsonTypeInfoIn = (JsonTypeInfo<IN>)context.GetTypeInfo(typeof(IN))!;

#if DEBUG
        string str = JsonSerializer.Serialize<IN>(obj, jsonTypeInfoIn);
#endif   

        using HttpResponseMessage response = await client.PutAsJsonAsync(requestUri, obj, jsonTypeInfoIn, cancellationToken);

#if DEBUG
        string res = response.Content.ReadAsStringAsync(cancellationToken).Result;
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        JsonTypeInfo<OUT> jsonTypeInfoOut = (JsonTypeInfo<OUT>)context.GetTypeInfo(typeof(OUT))!;
        var model = await response.Content.ReadFromJsonAsync<OUT>(jsonTypeInfoOut, cancellationToken);
        return model;
    }

    #endregion

    #region Post

    /// <summary>
    /// Sends a POST request to the specified URI with the provided object serialized as JSON and returns the response body deserialized as an object of type <typeparamref name="OUT"/>.
    /// </summary>
    /// <typeparam name="IN">The type of the request object.</typeparam>
    /// <typeparam name="OUT">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    internal async Task<OUT?> PostAsJsonAsync<IN, OUT>(string requestUri, IN obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNotConnected(client);

        JsonTypeInfo<IN> jsonTypeInfoIn = (JsonTypeInfo<IN>)context.GetTypeInfo(typeof(IN))!;

#if DEBUG
        string str = JsonSerializer.Serialize<IN>(obj, jsonTypeInfoIn);
#endif       

        using HttpResponseMessage response = await client.PostAsJsonAsync(requestUri, obj, jsonTypeInfoIn, cancellationToken);

#if DEBUG
        string res = response.Content.ReadAsStringAsync(cancellationToken).Result;
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        JsonTypeInfo<OUT> jsonTypeInfoOut = (JsonTypeInfo<OUT>)context.GetTypeInfo(typeof(OUT))!;
        var model = await response.Content.ReadFromJsonAsync<OUT>(jsonTypeInfoOut, cancellationToken);
        return model;
    }

    /// <summary>
    /// Sends a POST request to the specified URI with the provided object serialized as JSON.
    /// </summary>
    /// <typeparam name="IN">The type of the request object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task PostAsJsonAsync<IN>(string requestUri, IN obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNotConnected(client);

        JsonTypeInfo<IN> jsonTypeInfo = (JsonTypeInfo<IN>)context.GetTypeInfo(typeof(IN))!;

#if DEBUG
        string str = JsonSerializer.Serialize<IN>(obj, jsonTypeInfo);
#endif  

        using HttpResponseMessage response = await client.PostAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);

#if DEBUG
        string res = response.Content.ReadAsStringAsync(cancellationToken).Result;
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);
    }

    /// <summary>
    /// Sends a POST request to the specified URI and returns the response body deserialized as an object of type <typeparamref name="OUT"/>.
    /// </summary>
    /// <typeparam name="OUT">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<OUT?> PostFromJsonAsync<OUT>(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.PostAsync(requestUri, null, cancellationToken);

#if DEBUG
        string res = response.Content.ReadAsStringAsync(cancellationToken).Result;
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        JsonTypeInfo<OUT> jsonTypeInfoOut = (JsonTypeInfo<OUT>)context.GetTypeInfo(typeof(OUT))!;
        var model = await response.Content.ReadFromJsonAsync<OUT>(jsonTypeInfoOut, cancellationToken);
        return model;
    }

    /// <summary>
    /// Sends a POST request to the specified URI with the provided files and returns the response body deserialized as an object of type <typeparamref name="OUT"/>.
    /// </summary>
    /// <typeparam name="OUT">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="files">The files to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<OUT?> PostFilesFromJsonAsync<OUT>(string requestUri, IEnumerable<KeyValuePair<string, System.IO.Stream>> files, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
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

#if DEBUG
        string res = response.Content.ReadAsStringAsync(cancellationToken).Result;
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        JsonTypeInfo<OUT> jsonTypeInfoOut = (JsonTypeInfo<OUT>)context.GetTypeInfo(typeof(OUT))!;
        var model = await response.Content.ReadFromJsonAsync<OUT>(jsonTypeInfoOut, cancellationToken);
        return model;
    }

    #endregion

    #region Delete

    /// <summary>
    /// Sends a DELETE request to the specified URI with the provided object serialized as JSON and returns the response body deserialized as an object of type <typeparamref name="OUT"/>.
    /// </summary>
    /// <typeparam name="IN">The type of the request object.</typeparam>
    /// <typeparam name="OUT">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<OUT?> DeleteJsonAsync<IN, OUT>(string requestUri, IN obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNotConnected(client);

        JsonTypeInfo<IN> jsonTypeInfoIn = (JsonTypeInfo<IN>)context.GetTypeInfo(typeof(IN))!;

        string str = JsonSerializer.Serialize<IN>(obj, jsonTypeInfoIn);

        HttpRequestMessage requestMessage = new(HttpMethod.Delete, requestUri)
        {
            Content = new StringContent(str, Encoding.UTF8)
        };

        using HttpResponseMessage response = await client.SendAsync(requestMessage, cancellationToken);

#if DEBUG
        string res = response.Content.ReadAsStringAsync(cancellationToken).Result;
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        JsonTypeInfo<OUT> jsonTypeInfoOut = (JsonTypeInfo<OUT>)context.GetTypeInfo(typeof(OUT))!;
        var model = await response.Content.ReadFromJsonAsync<OUT>(jsonTypeInfoOut, cancellationToken);
        return model;
    }

    /// <summary>
    /// Sends a DELETE request to the specified URI and returns the response body deserialized as an object of type <typeparamref name="OUT"/>.
    /// </summary>
    /// <typeparam name="OUT">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member. This is automatically set by the compiler.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>

    protected async Task<OUT?> DeleteAsJsonAsync<OUT>(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNotConnected(client);

        using HttpResponseMessage response = await client.DeleteAsync(requestUri, cancellationToken);

#if DEBUG
        string res = response.Content.ReadAsStringAsync(cancellationToken).Result;
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        JsonTypeInfo<OUT> jsonTypeInfoOut = (JsonTypeInfo<OUT>)context.GetTypeInfo(typeof(OUT))!;
        var model = await response.Content.ReadFromJsonAsync<OUT>(jsonTypeInfoOut, cancellationToken);
        return model;
    }

    #endregion

    #region PATCH

    /// <summary>
    /// Sends a PATCH request to the specified URI with the provided object serialized as JSON and returns the response body deserialized as an object of type <typeparamref name="OUT"/>.
    /// </summary>
    /// <typeparam name="IN">The type of the request object.</typeparam>
    /// <typeparam name="OUT">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<OUT?> PatchAsJsonAsync<IN, OUT>(string requestUri, IN obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNotConnected(client);

        JsonTypeInfo<IN> jsonTypeInfoIn = (JsonTypeInfo<IN>)context.GetTypeInfo(typeof(IN))!;

#if DEBUG
        string req = JsonSerializer.Serialize<IN>(obj, jsonTypeInfoIn);
#endif      

        using HttpResponseMessage response = await client.PatchAsJsonAsync(requestUri, obj, jsonTypeInfoIn, cancellationToken);

#if DEBUG
        string res = response.Content.ReadAsStringAsync(cancellationToken).Result;
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        JsonTypeInfo<OUT> jsonTypeInfoOut = (JsonTypeInfo<OUT>)context.GetTypeInfo(typeof(OUT))!;
        var model = await response.Content.ReadFromJsonAsync<OUT>(jsonTypeInfoOut, cancellationToken);
        return model;
    }

    #endregion
}

