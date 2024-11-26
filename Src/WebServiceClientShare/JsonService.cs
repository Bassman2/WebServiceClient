namespace WebServiceClient;

public class JsonService(Uri host, JsonSerializerContext context, IAuthenticator? authenticator = null) : WebService(host, authenticator)
{
    protected readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        TypeInfoResolver = context,
        Converters =
        {
            new JsonDateTimeConverter()
        }
    };

    #region Get
        
    protected async Task<T?> GetFromJsonAsync<T>(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }

        return await ReadFromJsonAsync<T>(response, cancellationToken);
    }

    #endregion

    #region Put
    
    protected async Task PutAsJsonAsync<T>(string requestUri, T obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;
        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
    }

    protected async Task<T2?> PutAsJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;
        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }

        return await ReadFromJsonAsync<T2>(response, cancellationToken);
    }

    #endregion

    #region Post

    protected async Task<T2?> PostAsJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;
        using HttpResponseMessage response = await client!.PostAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
        return await ReadFromJsonAsync<T2>(response, cancellationToken);
    }

    protected async Task PostAsJsonAsync<T>(string requestUri, T obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;
        using HttpResponseMessage response = await client!.PostAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
    }

    protected async Task<T?> PostFromJsonAsync<T>(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.PostAsync(requestUri, null, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
        return await ReadFromJsonAsync<T>(response, cancellationToken);
    }

    protected async Task<T?> PostFilesFromJsonAsync<T>(string requestUri, IEnumerable<KeyValuePair<string, Stream>> files, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
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
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }
        return await ReadFromJsonAsync<T>(response, cancellationToken);
    }

    #endregion


    #region PATCH

    protected async Task<T2?> PatchAsJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;
        using HttpResponseMessage response = await client!.PatchAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }

        return await ReadFromJsonAsync<T2>(response, cancellationToken);
    }
    #endregion

    #region Json Helper

    protected async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return (T?) await response.Content.ReadFromJsonAsync(typeof(T), context, cancellationToken);
    }

    #endregion
}

