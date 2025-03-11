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
        client!.DefaultRequestHeaders.Add("Accept", "application/json");
        client!.DefaultRequestHeaders.Add("User-Agent", appName);
        this.context = context;
    }

    #region Get

    /// <summary>
    /// Sends a GET request to the specified URI and returns the response body deserialized as an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<T?> GetFromJsonAsync<T>(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);

#if DEBUG
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
#endif
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }

        var res = await ReadFromJsonAsync<T>(response, cancellationToken);
        return res;
    }

    #endregion

    #region Put

    /// <summary>
    /// Sends a PUT request to the specified URI with the provided object serialized as JSON.
    /// </summary>
    /// <typeparam name="T">The type of the request object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async Task PutAsJsonAsync<T>(string requestUri, T obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;

#if DEBUG
        string str = JsonSerializer.Serialize<T>(obj, jsonTypeInfo);
#endif   

        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
    }

    /// <summary>
    /// Sends a PUT request to the specified URI with the provided object serialized as JSON and returns the response body deserialized as an object of type <typeparamref name="T2"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the request object.</typeparam>
    /// <typeparam name="T2">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<T2?> PutAsJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;

#if DEBUG
        string str = JsonSerializer.Serialize<T1>(obj, jsonTypeInfo);
#endif   

        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }

        return await ReadFromJsonAsync<T2>(response, cancellationToken);
    }

    #endregion

    #region Post

    /// <summary>
    /// Sends a POST request to the specified URI with the provided object serialized as JSON and returns the response body deserialized as an object of type <typeparamref name="T2"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the request object.</typeparam>
    /// <typeparam name="T2">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    internal async Task<T2?> PostAsJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;

#if DEBUG
        string str = JsonSerializer.Serialize<T1>(obj, jsonTypeInfo);
#endif       

        using HttpResponseMessage response = await client!.PostAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
        var model = await ReadFromJsonAsync<T2>(response, cancellationToken);
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
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;

#if DEBUG
        string str = JsonSerializer.Serialize<T>(obj, jsonTypeInfo);
#endif  

        using HttpResponseMessage response = await client!.PostAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
    }

    /// <summary>
    /// Sends a POST request to the specified URI and returns the response body deserialized as an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<T?> PostFromJsonAsync<T>(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
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
        return await ReadFromJsonAsync<T>(response, cancellationToken);
    }

    /// <summary>
    /// Sends a POST request to the specified URI with the provided files and returns the response body deserialized as an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="files">The files to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<T?> PostFilesFromJsonAsync<T>(string requestUri, IEnumerable<KeyValuePair<string, System.IO.Stream>> files, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
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

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
        return await ReadFromJsonAsync<T>(response, cancellationToken);
    }

    #endregion

    #region Delete

    /// <summary>
    /// Sends a DELETE request to the specified URI with the provided object serialized as JSON and returns the response body deserialized as an object of type <typeparamref name="T2"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the request object.</typeparam>
    /// <typeparam name="T2">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<T2?> DeleteJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;

        string str = JsonSerializer.Serialize<T1>(obj, jsonTypeInfo);

        HttpRequestMessage requestMessage = new(HttpMethod.Delete, requestUri)
        {
            Content = new StringContent(str, Encoding.UTF8)
        };

        using HttpResponseMessage response = await client!.SendAsync(requestMessage, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }

        return await ReadFromJsonAsync<T2>(response, cancellationToken);
    }


    #endregion


    #region PATCH

    /// <summary>
    /// Sends a PATCH request to the specified URI with the provided object serialized as JSON and returns the response body deserialized as an object of type <typeparamref name="T2"/>.
    /// </summary>
    /// <typeparam name="T1">The type of the request object.</typeparam>
    /// <typeparam name="T2">The type of the response object.</typeparam>
    /// <param name="requestUri">The request URI.</param>
    /// <param name="obj">The object to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="memberName">The name of the calling member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response object.</returns>
    protected async Task<T2?> PatchAsJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;

#if DEBUG
        string req = JsonSerializer.Serialize<T1>(obj, jsonTypeInfo);
#endif      

        using HttpResponseMessage response = await client!.PatchAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);

#if DEBUG
        string res = await response.Content.ReadAsStringAsync(cancellationToken);
#endif

        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }

        return await ReadFromJsonAsync<T2>(response, cancellationToken);
        //return (T2?)await response.Content.ReadFromJsonAsync(typeof(T2), context, cancellationToken);
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
            return res;
        //}
        //catch(Exception e)
        //{
        //    throw;
        //}
    }

    #endregion
}

