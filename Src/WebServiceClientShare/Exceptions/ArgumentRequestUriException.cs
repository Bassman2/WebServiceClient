namespace WebServiceClient;

public class ArgumentRequestUriException : ArgumentException 
{
    public ArgumentRequestUriException(string message) : base(message)
    { }
    public ArgumentRequestUriException(string message, string? paramName)
        : base(message, paramName)
    { }


    public static new void ThrowIfNullOrWhiteSpace(string? argument, string? paramName)
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            throw new ArgumentRequestUriException("Argument is null or empty", paramName);
        }
    }
}
