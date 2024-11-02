using System.Text.Json.Serialization;

namespace WebServiceDepenencyConsole;

public class DemoModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)]
//[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(DemoModel))]
public partial class SourceGenerationContext : JsonSerializerContext
{
}
