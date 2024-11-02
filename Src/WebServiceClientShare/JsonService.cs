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
    
    protected string? GetString(string requestUri)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argument is null or empty", nameof(requestUri));
#endif
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
        return response.Content.ReadAsStringAsync().Result;
    }

    protected async Task<string?> GetStringAsync(string requestUri)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argument is null or empty", nameof(requestUri));
#endif
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri);
        string str = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
        return await response.Content.ReadAsStringAsync();
    }

    protected T? GetFromJson<T>(string requestUri)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argument is null or empty", nameof(requestUri));
#endif
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }

        return ReadFromJson<T>(response);
    }

    protected async Task<T?> GetFromJsonAsync<T>(string requestUri)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argument is null or empty", nameof(requestUri));
#endif
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri);
        string str = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }

        return await ReadFromJsonAsync<T>(response);
    }

    protected T? PutAsJson<T>(string requestUri, T obj)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argument is null or empty", nameof(requestUri));
#endif
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
            ErrorHandling(response);
        }

        return ReadFromJson<T>(response);
    }

    protected async Task<T?> PutAsJsonAsync<T>(string requestUri, T obj)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argument is null or empty", nameof(requestUri));
#endif
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;
        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo);
#else
        using HttpResponseMessage response = await client!.PutAsJsonAsync(requestUri, obj, jsonSerializerOptions);
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }

        return await ReadFromJsonAsync<T>(response);
    }

    protected void PostAsJson<T>(string requestUri, T obj)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argument is null or empty", nameof(requestUri));
#endif
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;
        using HttpResponseMessage response = client!.PostAsJsonAsync(requestUri, obj, jsonTypeInfo).Result;
#else
        using HttpResponseMessage response = client!.PostAsJsonAsync(requestUri, obj, jsonSerializerOptions).Result;
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
    }

    protected async Task PostAsJsonAsync<T>(string requestUri, T obj)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argument is null or empty", nameof(requestUri));
#endif
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

#if NET8_0_OR_GREATER
        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;
        using HttpResponseMessage response = await client!.PostAsJsonAsync(requestUri, obj, jsonTypeInfo);
#else
        using HttpResponseMessage response = await client!.PostAsJsonAsync(requestUri, obj, jsonSerializerOptions);
#endif
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
    }

    protected Stream GetFromStream(string requestUri)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argument is null or empty", nameof(requestUri));
#endif
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
        var stream = new MemoryStream();
        response.Content.CopyToAsync(stream).Wait();
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    protected async Task<Stream> GetFromStreamAsync(string requestUri)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argument is null or empty", nameof(requestUri));
#endif
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri);
        string str = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
        var stream = new MemoryStream();
        await response.Content.CopyToAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);
        return stream;
    }

    protected void Delete(string requestUri)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argemtn is null or empty", nameof(requestUri));
#endif
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.DeleteAsync(requestUri).Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
    }

    protected async Task DeleteAsync(string requestUri)
    {
#if NET8_0_OR_GREATER
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
#else
        if (string.IsNullOrEmpty(requestUri)) throw new ArgumentException("The argemtn is null or empty", nameof(requestUri));
#endif
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.DeleteAsync(requestUri);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
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

    private async Task<T?> ReadFromJsonAsync<T>(HttpResponseMessage response)
    {
#if NET8_0_OR_GREATER
        return (T?) await response.Content.ReadFromJsonAsync(typeof(T), context);
#else
        return await response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions);
#endif
    }
}

