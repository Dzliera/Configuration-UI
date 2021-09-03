using ConfigurationUi.Abstractions;
using NJsonSchema;

namespace ConfigurationUi.Models
{
    internal class ConfigurationContext
    {
        public IConfigurationStorage Storage { get; set; }
        public JsonSchema Scheme { get; set; }
    }
}