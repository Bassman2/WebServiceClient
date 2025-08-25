namespace WebServiceClient.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class ModelPropertyParametersAttribute : Attribute
{
    public string? JsonPropertyName { get; set; }
}
