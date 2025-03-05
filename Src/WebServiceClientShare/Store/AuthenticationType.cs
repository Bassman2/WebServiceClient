namespace WebServiceClient.Store;

/// <summary>
/// Specifies the type of authentication used in the web service client.
/// </summary>
public enum AuthenticationType
{
    /// <summary>
    /// Bearer token authentication.
    /// </summary>
    Bearer,

    /// <summary>
    /// Token-based authentication.
    /// </summary>
    Token,

    /// <summary>
    /// Login-based authentication.
    /// </summary>
    Login
}
