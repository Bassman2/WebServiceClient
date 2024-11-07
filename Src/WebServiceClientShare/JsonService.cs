#if NET8_0_OR_GREATER
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

    protected async Task<string?> GetStringAsync(string requestUri, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri);
        string str = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        return await response.Content.ReadAsStringAsync();
    }

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

    protected async Task<T?> GetFromJsonAsync<T>(string requestUri, CancellationToken cancellationToken = default, [CallerMemberName] string memberName = "")
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

    protected async Task<T2?> PutAsJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken = default, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;
        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo);
#else
        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonSerializerOptions);
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }

        return await ReadFromJsonAsync<T2>(response);
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

    protected async Task<T2?> PostAsJsonAsync<T1, T2>(string requestUri, T1 obj, CancellationToken cancellationToken = default, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T1> jsonTypeInfo = (JsonTypeInfo<T1>)context.GetTypeInfo(typeof(T1))!;
        using HttpResponseMessage response = await client!.PostAsJsonAsync(requestUri, obj, jsonTypeInfo);
#else
        using HttpResponseMessage response = await client!.PostAsJsonAsync(requestUri, obj, jsonSerializerOptions);
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        return await ReadFromJsonAsync<T2>(response);
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

    protected async Task<Stream> GetFromStreamAsync(string requestUri, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri);
        string str = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        var stream = new MemoryStream();
        await response.Content.CopyToAsync(stream);
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

    protected async Task DownloadAsync(Uri requestUri, string filePath, [CallerMemberName] string memberName = "")
    {
        //ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri);
        string str = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        using var file = File.Create(filePath);
        await response.Content.CopyToAsync(file);
    }

    protected async Task DownloadAsync(string requestUri, string filePath, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri);
        string str = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
        using var file = File.Create(filePath);
        await response.Content.CopyToAsync(file);
    }

    protected bool Delete(string requestUri, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.DeleteAsync(requestUri).Result;
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
        if (!response.IsSuccessStatusCode)
        {
             ErrorHandling(response, memberName);
        }
        return true;
    }

    protected async Task DeleteAsync(string requestUri, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.DeleteAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }
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

    private async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
#if NET8_0_OR_GREATER
        return (T?) await response.Content.ReadFromJsonAsync(typeof(T), context, cancellationToken);
#else
        return await response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions, cancellationToken);
#endif
    }
}

