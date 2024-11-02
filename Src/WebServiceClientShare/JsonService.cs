using System.Text.Json.Serialization.Metadata;

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
    
    protected string? GetString(string requestUri)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
        return response.Content.ReadAsStringAsync().Result;
    }

    protected T? GetFromJson<T>(string requestUri)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);
        
        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
        Type x = typeof(T);
        //return response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions).Result;
        return (T?)response.Content.ReadFromJsonAsync(typeof(T), context).Result;
    }

    protected T? PutAsJson<T>(string requestUri, T obj)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;

        //using HttpResponseMessage response = client!.PutAsJsonAsync(requestUri, obj, jsonSerializerOptions).Result;
        using HttpResponseMessage response = client!.PutAsJsonAsync(requestUri, obj, jsonTypeInfo).Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
        //return response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions).Result;

        //return response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions).Result;
        //return (T?)response.Content.ReadFromJsonAsync(typeof(T), context).Result;
        return response.Content.ReadFromJsonAsync<T>(jsonTypeInfo).Result;

    }

    protected void PostAsJson<T>(string requestUri, T obj)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        JsonTypeInfo<T> jsonTypeInfo = (JsonTypeInfo<T>)context.GetTypeInfo(typeof(T))!;

        //using HttpResponseMessage response = client!.PostAsJsonAsync(requestUri, obj, jsonSerializerOptions).Result;
        using HttpResponseMessage response = client!.PostAsJsonAsync(requestUri, obj, jsonTypeInfo).Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
    }

    protected Stream GetFromStream(string requestUri)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
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

    protected void Delete(string requestUri)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.DeleteAsync(requestUri).Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
    }
}
