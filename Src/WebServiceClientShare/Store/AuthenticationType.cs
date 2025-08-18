namespace WebServiceClient.Store;

/// <summary>
/// Specifies the type of authentication used in the web service client.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<AuthenticationType>))]
public enum AuthenticationType
{
    /// <summary>
    /// No authentication.
    /// </summary>
    None,

    /// <summary>
    /// Basic authentication.
    /// </summary>
    Basic,

    /// <summary>
    /// Bearer token authentication.
    /// </summary>
    Bearer,

    ///// <summary>
    ///// Token-based authentication.
    ///// </summary>
    //Token,

    ///// <summary>
    ///// Login-based authentication.
    ///// </summary>
    //Login,

    /// <summary>
    /// Bearer token authentication combined with JFrog API authentication.
    /// </summary>
    BearerAndJFrogApi
}
