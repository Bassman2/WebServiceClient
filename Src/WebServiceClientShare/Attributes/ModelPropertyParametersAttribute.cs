namespace WebServiceClient.Attributes;

/// <summary>
/// Attribute to specify additional parameters for a model property, such as the JSON property name.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ModelPropertyParametersAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the JSON property name for the model property.
    /// </summary>
    public string? JsonPropertyName { get; set; }
}
