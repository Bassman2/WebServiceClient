namespace WebServiceClient;

// https://www.postman.com/

public abstract class JsonService : WebService
{
    private readonly JsonSerializerContext context;

    public JsonService(Uri host, IAuthenticator? authenticator, string appName, JsonSerializerContext context)
        : base(host, authenticator, appName)
    {
        client!.DefaultRequestHeaders.Add("Accept", "application/json");
        client!.DefaultRequestHeaders.Add("User-Agent", appName);
        this.context = context;
    }

    #region Get
        
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

#if DEBUG
        string str = JsonSerializer.Serialize<T>(obj, jsonTypeInfo);
#endif   

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
    }
    #endregion

    #region Json Helper

    protected async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return (T?) await response.Content.ReadFromJsonAsync(typeof(T), context, cancellationToken);
    }

    #endregion
}

