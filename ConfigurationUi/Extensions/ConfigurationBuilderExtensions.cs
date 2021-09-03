using ConfigurationUi.Options.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ConfigurationUi.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static void AddConfigurationUi<TConfigModel>(this IConfigurationBuilder configurationBuilder, string filePath)
        {
            var optionsBuilder = new ConfigurationUiOptionsBuilder();
            optionsBuilder.UseJsonFileStorage(filePath).WithSchemeFromType<TConfigModel>();
            configurationBuilder.Sources.Insert(0, new JsonConfigurationSource(){Path = filePath});
        }
    }
}