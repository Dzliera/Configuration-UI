using NJsonSchema;

namespace ConfigurationUi.Server.Api.Models;

public class GetConfigurationResponse
{
    public JsonSchema4 Schema { get; set; } = null!;
    public Dictionary<string, string> Configuration { get; set; } = null!;
}