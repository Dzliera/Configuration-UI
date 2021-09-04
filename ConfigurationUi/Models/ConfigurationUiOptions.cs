using ConfigurationUi.Abstractions;
using NJsonSchema;

namespace ConfigurationUi.Models
{
    internal class ConfigurationUiOptions
    {
        public IConfigurationStorage Storage { get; set; }
        public JsonSchema Schema { get; set; }
        
        public string WebUiPath { get; set; }
    }
}