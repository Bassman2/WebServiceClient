namespace WebServiceClient;

/// <summary>
/// Represents an abstract base class for JSON-based web services, providing common functionality for HTTP operations.
/// </summary>
public abstract class JsonService : WebService
{
    private readonly JsonSerializerContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonService"/> class with the specified host, authenticator, application name, and JSON serializer context.
    /// </summary>
    /// <param name="host">The base URI of the web service.</param>
    /// <param name="authenticator">The authenticator used to authenticate the web service client.</param>
    /// <param name="appName">The name of the application.</param>
    /// <param name="context">The JSON serializer context.</param>
    public JsonService(Uri host, IAuthenticator? authenticator, string appName, JsonSerializerContext context)
        : base(host, authenticator, appName)
    {
        WebServiceException.ThrowIfNotConnected(client);

        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("User-Agent", appName);
        this.context = context;
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
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        var res = await ReadFromJsonAsync<OUT>(response, cancellationToken);
        return res;
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

        JsonTypeInfo<IN> jsonTypeInfo = (JsonTypeInfo<IN>)context.GetTypeInfo(typeof(IN))!;

#if DEBUG
        string str = JsonSerializer.Serialize<IN>(obj, jsonTypeInfo);
#endif   

        using HttpResponseMessage response = await client.PutAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);
        return await ReadFromJsonAsync<OUT>(response, cancellationToken);
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

        JsonTypeInfo<IN> jsonTypeInfo = (JsonTypeInfo<IN>)context.GetTypeInfo(typeof(IN))!;

#if DEBUG
        string str = JsonSerializer.Serialize<IN>(obj, jsonTypeInfo);
#endif       

        using HttpResponseMessage response = await client.PostAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        var model = await ReadFromJsonAsync<OUT>(response, cancellationToken);
        return model;
    }

    /// <summary>
    /// Sends a POST request to the specified URI with the provided object serialized as JSON.
    /// </summary>
    /// <typeparam name="T">The type of the request object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task PostAsJsonAsync<T>(string requestUri, T obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNotConnected(client);

        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;

#if DEBUG
        string str = JsonSerializer.Serialize<T>(obj, jsonTypeInfo);
#endif  

        using HttpResponseMessage response = await client.PostAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
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
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        return await ReadFromJsonAsync<OUT>(response, cancellationToken);
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
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        return await ReadFromJsonAsync<OUT>(response, cancellationToken);
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

        JsonTypeInfo<IN> jsonTypeInfo = (JsonTypeInfo<IN>)context.GetTypeInfo(typeof(IN))!;

        string str = JsonSerializer.Serialize<IN>(obj, jsonTypeInfo);

        HttpRequestMessage requestMessage = new(HttpMethod.Delete, requestUri)
        {
            Content = new StringContent(str, Encoding.UTF8)
        };

        using HttpResponseMessage response = await client.SendAsync(requestMessage, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        return await ReadFromJsonAsync<OUT>(response, cancellationToken);
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
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        return await ReadFromJsonAsync<OUT>(response, cancellationToken);
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

        JsonTypeInfo<IN> jsonTypeInfo = (JsonTypeInfo<IN>)context.GetTypeInfo(typeof(IN))!;

#if DEBUG
        string req = JsonSerializer.Serialize<IN>(obj, jsonTypeInfo);
#endif      

        using HttpResponseMessage response = await client.PatchAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        await ErrorCheckAsync(response, memberName, cancellationToken);

        return await ReadFromJsonAsync<OUT>(response, cancellationToken);
    }

    #endregion

    #region Json Helper

    /// <summary>
    /// Reads the JSON content from the response and deserializes it as an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the response object.</typeparam>
    /// <param name="response">The HTTP response message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        //try
        //{
        var res = (T?)await response.Content.ReadFromJsonAsync(typeof(T), context, cancellationToken);

        //JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;

        

        //var res = (T?)await response.Content.ReadFromJsonAsync<T>(jsonTypeInfo, cancellationToken);
        return res;
        //}
        //catch(Exception e)
        //{
        //    throw;
        //}
    }

    /*
    private static async Task<object?> ReadFromJsonAsyncCore(HttpContent content, Type type, JsonSerializerContext context, CancellationToken cancellationToken)
    {
        using (Stream contentStream = await GetContentStreamAsync(content, cancellationToken).ConfigureAwait(false))
        {
            return await JsonSerializer.DeserializeAsync(contentStream, type, context, cancellationToken).ConfigureAwait(false);
        }
    }
    */
    #endregion
}

