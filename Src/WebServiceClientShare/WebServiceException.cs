namespace WebServiceClient;

public class WebServiceException : Exception
{
    public WebServiceException(string message) : base(message)
    { }
    public WebServiceException(Uri? requestUri, HttpStatusCode statusCode, string? reasonPhrase) 
        : base($"{statusCode} {reasonPhrase}: \"{requestUri}\"")
    { }

    public WebServiceException(string? message, Uri? requestUri, HttpStatusCode statusCode, string? reasonPhrase) 
        : base($"{statusCode} {reasonPhrase}: \"{requestUri}\" {message}")
    { }

    public static void ThrowIfNullOrNotConnected(JsonService? service)
    {
        if (service == null)
        {
            throw new WebServiceException("JsonService is null");
        }
        if (!service.IsConnected)
        {
            throw new WebServiceException("JsonService is not connected");
        }
    }
}
