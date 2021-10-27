using NJsonSchema;

namespace ConfigurationUi.Abstractions
{
    public interface IConfigurationSchemeProvider
    {
        public JsonSchema4 GetConfigurationScheme();
    }

}