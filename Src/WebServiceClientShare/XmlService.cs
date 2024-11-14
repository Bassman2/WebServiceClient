



namespace WebServiceClient;

#if NET8_0_OR_GREATER
public class XmlService(Uri host, IAuthenticator? authenticator = null) : WebService(host, authenticator)
{
#else
public class XmlService : WebService
{
    public XmlService(Uri host, IAuthenticator? authenticator = null) 
    : base(host, authenticator)
    {}
#endif

    [RequiresUnreferencedCode("Calls System.Xml.Serialization.XmlSerializer.XmlSerializer(Type)")]
    protected T? GetFromXml<T>(string requestUri, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = client!.GetAsync(requestUri).Result;
        string str = response.Content.ReadAsStringAsync().Result;
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }

        return ReadFromXml<T>(response);
    }

    [RequiresUnreferencedCode("Calls System.Xml.Serialization.XmlSerializer.XmlSerializer(Type)")]
    protected async Task<T?> GetFromXmlAsync<T>(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            ErrorHandling(response, memberName);
        }

        return await ReadFromXmlAsync<T>(response, cancellationToken);
    }

    [RequiresUnreferencedCode("Calls System.Xml.Serialization.XmlSerializer.XmlSerializer(Type)")]
    private static T? ReadFromXml<T>(HttpResponseMessage response)
    {
        string str = response.Content.ReadAsStringAsync().Result;

        var serializer = new XmlSerializer(typeof(T));
        using (var reader = new StringReader(str))
        {
            T? val = (T?)serializer.Deserialize(reader);
            return val;
        }
    }

    [RequiresUnreferencedCode("Calls System.Xml.Serialization.XmlSerializer.XmlSerializer(Type)")]
    private static async Task<T?> ReadFromXmlAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        string str = await response.Content.ReadAsStringAsync(cancellationToken);

        var serializer = new XmlSerializer(typeof(T));
        using (var reader = new StringReader(str))
        {
            T? val = (T?)serializer.Deserialize(reader);
            return val;
        }
    }
}