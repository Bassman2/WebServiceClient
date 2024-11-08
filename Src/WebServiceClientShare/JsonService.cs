﻿#if NET8_0_OR_GREATER
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;
#endif

namespace WebServiceClient;

#if NET8_0_OR_GREATER
public class JsonService(Uri host, JsonSerializerContext context, IAuthenticator? authenticator = null) : WebService(host, authenticator)
{
#else
public class JsonService : WebService
{
    public JsonService(Uri host, IAuthenticator? authenticator = null) 
    : base(host, authenticator)
    {}

#endif
    protected readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
#if NET8_0_OR_GREATER
        TypeInfoResolver = context,
#endif
        Converters =
        {
            new JsonDateTimeConverter()
        }
    };
    
    protected T? GetFromJson<T>(string requestUri, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }

        return ReadFromJson<T>(response);
    }

    protected async Task<T?> GetFromJsonAsync<T>(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }

        return await ReadFromJsonAsync<T>(response, cancellationToken);
    }

    protected void PutAsJson<T>(string requestUri, T obj, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;
        using HttpResponseMessage response = client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo).Result;
#else
        using HttpResponseMessage response = client!.PutAsJsonAsync(requestUri, obj, jsonSerializerOptions).Result;
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
    }

    protected async Task PutAsJsonAsync<T>(string requestUri, T obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;
        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);
#else
        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonSerializerOptions, cancellationToken);
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
    }

    protected T2? PutAsJson<T1, T2 >(string requestUri, T1 obj, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;
        using HttpResponseMessage response = client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo).Result;
#else
        using HttpResponseMessage response = client!.PutAsJsonAsync(requestUri, obj, jsonSerializerOptions).Result;
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }

        return ReadFromJson<T2>(response);
    }

    protected async Task<T2?> PutAsJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;
        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);
#else
        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonSerializerOptions, cancellationToken);
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }

        return await ReadFromJsonAsync<T2>(response, cancellationToken);
    }

    public T2? PostAsJson<T1, T2>(string requestUri, T1 obj, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;
        using HttpResponseMessage response = client!.PostAsJsonAsync(requestUri, obj, jsonTypeInfo).Result;
#else
        using HttpResponseMessage response = client!.PostAsJsonAsync(requestUri, obj, jsonSerializerOptions).Result;
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        return ReadFromJson<T2>(response);
    }

    protected async Task<T2?> PostAsJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;
        using HttpResponseMessage response = await client!.PostAsJsonAsync(requestUri, obj, jsonTypeInfo, cancellationToken);
#else
        using HttpResponseMessage response = await client!.PostAsJsonAsync(requestUri, obj, jsonSerializerOptions, cancellationToken);
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        return await ReadFromJsonAsync<T2>(response, cancellationToken);
    }

    public T? PostFilesFromJson<T>(string requestUri, IEnumerable<KeyValuePair<string, Stream>> files, [CallerMemberName] string memberName = "")
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
        return ReadFromJson<T>(response);
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
            ErrorHandling(response, memberName);
        }
        return await ReadFromJsonAsync<T>(response, cancellationToken);
    }

    

    

   



    private T? ReadFromJson<T>(HttpResponseMessage response)
    {
#if NET8_0_OR_GREATER

        //JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;
        //return response.Content.ReadFromJsonAsync<T>(jsonTypeInfo).Result;

        return (T?)response.Content.ReadFromJsonAsync(typeof(T), context).Result;
#else
        return response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions).Result;
#endif
    }

    private async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
#if NET8_0_OR_GREATER
        return (T?) await response.Content.ReadFromJsonAsync(typeof(T), context, cancellationToken);
#else
        return await response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions, cancellationToken);
#endif
    }
}

