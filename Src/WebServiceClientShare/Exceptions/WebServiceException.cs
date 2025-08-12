namespace WebServiceClient;

/// <summary>
/// Represents an exception that occurs during web service operations.
/// </summary>
public class WebServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebServiceException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public WebServiceException(string? message) : base(message)
    { }

    ///// <summary>
    ///// Initializes a new instance of the <see cref="WebServiceException"/> class with a specified error message, request URI, and HTTP status code.
    ///// </summary>
    ///// <param name="requestUri">The URI of the web request that caused the error.</param>
    ///// <param name="statusCode">The HTTP status code returned by the web service.</param>
    ///// <param name="reasonPhrase">The reason phrase returned by the web service.</param>
    //public WebServiceException(Uri? requestUri, HttpStatusCode statusCode, string? reasonPhrase) 
    //    : base($"{statusCode} {reasonPhrase}: \"{requestUri}\"")
    //{
    //    this.StatusCode = statusCode;
    //}

    ///// <summary>
    ///// Initializes a new instance of the <see cref="WebServiceException"/> class with a specified error message, request URI, HTTP status code, and reason phrase.
    ///// </summary>
    ///// <param name="message">The message that describes the error.</param>
    ///// <param name="requestUri">The URI of the web request that caused the error.</param>
    ///// <param name="statusCode">The HTTP status code returned by the web service.</param>
    ///// <param name="reasonPhrase">The reason phrase returned by the web service.</param>
    //public WebServiceException(string? message, Uri? requestUri, HttpStatusCode statusCode, string? reasonPhrase)
    //    : base($"{statusCode} {reasonPhrase}: \"{requestUri}\" {message}")
    //{
    //    this.StatusCode = statusCode;
    //}

    /// <summary>
    /// Initializes a new instance of the <see cref="WebServiceException"/> class with a specified error message, request URI, HTTP status code, reason phrase, and member name.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="requestUri">The URI of the web request that caused the error.</param>
    /// <param name="statusCode">The HTTP status code returned by the web service.</param>
    /// <param name="reasonPhrase">The reason phrase returned by the web service.</param>
    /// <param name="memberName">The name of the member that caused the error.</param>
    public WebServiceException(string? message, Uri? requestUri, HttpStatusCode statusCode, string? reasonPhrase, string memberName) 
        : base(message)
    {
        this.RequestUri = requestUri;
        this.StatusCode = statusCode;
        this.ReasonPhrase = reasonPhrase;
        this.MemberName = memberName;
    }

    /// <summary>
    /// Gets the URI of the web request that caused the error.
    /// </summary>
    public Uri? RequestUri { get; }

    /// <summary>
    /// Gets the HTTP status code returned by the web service.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// Gets the reason phrase returned by the web service.
    /// </summary>
    public string? ReasonPhrase { get; }


    /// <summary>
    /// Gets the name of the member that caused the error.
    /// </summary>
    public string? MemberName { get; }

    /// <summary>
    /// Returns a string that represents the current <see cref="WebServiceException"/>.
    /// </summary>
    /// <returns>A string representation of the current exception.</returns>
    public override string ToString()
    {
        return $"{base.ToString()}, RequestUri: {RequestUri}, StatusCode: {StatusCode}, ReasonPhrase: {ReasonPhrase}, MemberName: {MemberName}";
    }

    /// <summary>
    /// Throws a <see cref="WebServiceException"/> if the specified web service is null or not connected.
    /// </summary>
    /// <param name="service">The web service to check.</param>
    /// <exception cref="WebServiceException">Thrown if the web service is null or not connected.</exception>
    public static void ThrowIfNullOrNotConnected([NotNull] WebService? service)
    {
        if (service == null)
        {
            throw new WebServiceException("WebService is null");
        }
        if (!service.IsConnected)
        {
            throw new WebServiceException("WebService is not connected");
        }
    }

    /// <summary>
    /// Throws a <see cref="WebServiceException"/> if the specified HTTP client is null, indicating that the web service is not connected.
    /// </summary>
    /// <param name="client">The HTTP client to check.</param>
    /// <exception cref="WebServiceException">Thrown if the HTTP client is null.</exception>
    public static void ThrowIfNotConnected([NotNull] HttpClient? client)
    {
        _ = client ?? throw new WebServiceException("WebService is not connected");
        //if (client == null)
        //{
        //    throw new WebServiceException("WebService is not connected");
        //}
    }

    /// <summary>
    /// Throws a <see cref="WebServiceException"/> with the specified message and HTTP response.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="response">The HTTP response that caused the error.</param>
    /// <param name="memberName">The name of the member that caused the error.</param>
    /// <exception cref="WebServiceException">Always thrown with the specified message and HTTP response details.</exception>
    public static void ThrowHttpError(string? message, HttpResponseMessage response, string memberName)
    {
        throw new WebServiceException(message, response.RequestMessage?.RequestUri, response.StatusCode, response.ReasonPhrase, memberName);
    }
}
