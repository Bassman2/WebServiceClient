namespace WebServiceClient.Attributes;

/// <summary>
/// Specifies that a property should be ignored by the model processing logic.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ModelIgnoreAttribute : Attribute
{
}
