using ConfigurationUi.Server.Abstractions;
using NJsonSchema;

namespace ConfigurationUi.Server.SchemeProviders
{
    /// <summary>
    /// Configuration scheme provider which reflect json scheme from CRL type
    /// </summary>
    /// <typeparam name="TSourceType"></typeparam>
    public class TypeSchemeProvider<TSourceType> : IConfigurationSchemeProvider
    {
        /// <inheritdoc />
        public JsonSchema4 GetConfigurationScheme()
        {
            return JsonSchema4.FromTypeAsync<TSourceType>().Result;
        }
    }
}