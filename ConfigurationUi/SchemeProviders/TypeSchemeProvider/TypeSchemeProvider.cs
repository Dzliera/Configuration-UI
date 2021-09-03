using ConfigurationUi.Abstractions;
using NJsonSchema;

namespace ConfigurationUi.SchemeProviders
{
    public class TypeSchemeProvider<TSourceType> : IConfigurationSchemeProvider
    {
        public JsonSchema GetConfigurationScheme()
        {
            return JsonSchema.FromType<TSourceType>();
        }
    }
}