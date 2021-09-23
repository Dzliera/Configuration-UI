using ConfigurationUi.Abstractions;
using NJsonSchema;

namespace ConfigurationUi.SchemeProviders
{
    public class TypeSchemeProvider<TSourceType> : IConfigurationSchemeProvider
    {
        public JsonSchema4 GetConfigurationScheme()
        {
            return JsonSchema4.FromTypeAsync<TSourceType>().Result;
        }
    }
}