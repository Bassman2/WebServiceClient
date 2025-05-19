namespace WebServiceClient;

/// <summary>
/// Exception thrown when the request URI argument is invalid.
/// </summary>
public class ArgumentRequestUriException : ArgumentException 
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentRequestUriException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ArgumentRequestUriException(string message) : base(message)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentRequestUriException"/> class with a specified error message and the name of the parameter that caused this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    public ArgumentRequestUriException(string message, string? paramName)
        : base(message, paramName)
    { }

    
    /// <summary>
    /// Throws an <see cref="ArgumentRequestUriException"/> if the specified argument is null or consists only of white-space characters.
    /// </summary>
    /// <param name="argument">The string argument to check.</param>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    /// <exception cref="ArgumentRequestUriException">Thrown when the argument is null or white-space.</exception>
    public static new void ThrowIfNullOrWhiteSpace(string? argument, string? paramName)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentRequestUriException("Argument is null or empty", paramName);
        }
    }

    public static void ThrowIfNullOrWhiteSpace(Uri? argument, string? paramName)
    {
        if (string.IsNullOrWhiteSpace(argument?.ToString()))
        {
            throw new ArgumentRequestUriException("Argument is null or empty", paramName);
        }
    }
}
