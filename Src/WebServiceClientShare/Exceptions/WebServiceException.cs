﻿namespace WebServiceClient;

public class WebServiceException : Exception
{
    public WebServiceException(string message) : base(message)
    { }

    //public WebServiceException(Uri? requestUri, HttpStatusCode statusCode, string? reasonPhrase) 
    //    : base($"{statusCode} {reasonPhrase}: \"{requestUri}\"")
    //{
    //    this.StatusCode = statusCode;
    //}

    //public WebServiceException(string? message, Uri? requestUri, HttpStatusCode statusCode, string? reasonPhrase)
    //    : base($"{statusCode} {reasonPhrase}: \"{requestUri}\" {message}")
    //{
    //    this.StatusCode = statusCode;
    //}

    public WebServiceException(string? message, Uri? requestUri, HttpStatusCode statusCode, string? reasonPhrase, string memberName) 
        : base($"{statusCode} {reasonPhrase}: \"{requestUri}\" {message} from {memberName}")
    {
        this.StatusCode = statusCode;
    }

    public HttpStatusCode StatusCode { get; }

    public static void ThrowIfNullOrNotConnected([NotNull]WebService? service)
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

    public static void ThrowHttpError(string? messagage, HttpResponseMessage response, string memberName)
    {
        throw new WebServiceException(messagage, response.RequestMessage?.RequestUri, response.StatusCode, response.ReasonPhrase, memberName);
    }
}
