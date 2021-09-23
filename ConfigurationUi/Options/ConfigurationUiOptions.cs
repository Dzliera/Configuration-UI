using ConfigurationUi.Abstractions;
using NJsonSchema;

namespace ConfigurationUi.Options
{
    internal class ConfigurationUiOptions
    {
        public IConfigurationStorageProvider StorageProvider { get; set; }
        public JsonSchema4 Schema { get; set; }
        
        public string WebUiPath { get; set; }
    }
}