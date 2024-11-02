namespace WebServiceClient;

public class ArgumentRequestUriException : ArgumentException 
{
    public ArgumentRequestUriException(string message) : base(message)
    { }
    public ArgumentRequestUriException(string message, string? paramName)
        : base(message, paramName)
    { }


#if NET8_0_OR_GREATER
    public static new void ThrowIfNullOrWhiteSpace(string? argument, string? paramName)
#else
    public static void ThrowIfNullOrWhiteSpace(string? argument, string? paramName)

#endif
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentRequestUriException("Argument is null or empty", paramName);
        }
    }
}
