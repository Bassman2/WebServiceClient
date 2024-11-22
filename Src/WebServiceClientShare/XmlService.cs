namespace WebServiceClient;

public class XmlService(Uri host, IAuthenticator? authenticator = null) : WebService(host, authenticator)
{
    [RequiresUnreferencedCode("Calls System.Xml.Serialization.XmlSerializer.XmlSerializer(Type)")]
    protected async Task<T?> GetFromXmlAsync<T>(string requestUri, CancellationToken cancellationToken, [CallerMemberName] string memberName = "")
    {
        ArgumentRequestUriException.ThrowIfNullOrWhiteSpace(requestUri, nameof(requestUri));
        WebServiceException.ThrowIfNullOrNotConnected(this);

        using HttpResponseMessage response = await client!.GetAsync(requestUri, cancellationToken);
        string str = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            await ErrorHandlingAsync(response, memberName, cancellationToken);
        }

        return await ReadFromXmlAsync<T>(response, cancellationToken);
    }

   

    [RequiresUnreferencedCode("Calls System.Xml.Serialization.XmlSerializer.XmlSerializer(Type)")]
    private static async Task<T?> ReadFromXmlAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        string str = await response.Content.ReadAsStringAsync(cancellationToken);

        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(str);
        T? val = (T?)serializer.Deserialize(reader);
        return val;
    }
}