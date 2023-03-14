using NJsonSchema;

namespace ConfigurationUi.Models
{
    internal class GetConfigurationResponse
    {
        public JsonSchema4 Schema { get; set; } = null!;
        public dynamic Configuration { get; set; }
    }
}