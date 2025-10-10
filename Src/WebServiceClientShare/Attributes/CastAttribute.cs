namespace WebServiceClient.Attributes;

/// <summary>
/// Specifies the class name to be used for casting.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CastAttribute(string className) : Attribute
{
    /// <summary>
    /// Gets or sets the class name for casting.
    /// </summary>
    public string ClassName { get; set; } = className;
}
