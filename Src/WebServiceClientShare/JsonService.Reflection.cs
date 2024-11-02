namespace WebServiceClient;

#if  false

public class JsonService : WebService
{
    public JsonService(Uri host, IAuthenticator? authenticator = null) 
    : base(host, authenticator)
    {}

    protected readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        Converters =
        {
            new JsonDateTimeConverter()
        }
    };

    protected string? GetString(string requestUri)
    {
        ArgumentException_ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
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
        ArgumentException_ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
        return response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions).Result;
    }

    protected T? PutAsJson<T>(string requestUri, T obj)
    {
        ArgumentException_ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.PutAsJsonAsync(requestUri, obj, jsonSerializerOptions).Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
        return response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions).Result;
    }

    protected void PostAsJson<T>(string requestUri, T obj)
    {
        ArgumentException_ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.PostAsJsonAsync(requestUri, obj, jsonSerializerOptions).Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
    }

    protected Stream GetFromStream(string requestUri)
    {
        ArgumentException_ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
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
        ArgumentException_ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.DeleteAsync(requestUri).Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response);
        }
    }

    private static void ArgumentException_ThrowIfNullOrWhiteSpace(string? argument, string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentException("", paramName);
        }
    }
}
#endif

