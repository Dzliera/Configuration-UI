using ConfigurationUi.Options.Builder;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConfigurationUi.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static void AddConfigurationUi<TConfigModel>(this IHostBuilder hostBuilder, string filePath)
        {
            var optionsBuilder = new ConfigurationUiOptionsBuilder();
            optionsBuilder.UseJsonFileStorage(filePath).WithSchemeFromType<TConfigModel>();
            
            hostBuilder.ConfigureAppConfiguration((_, builder) =>
            {
                builder.Sources.Insert(0, new JsonConfigurationSource() { Path = filePath });
            });

            hostBuilder.ConfigureServices((_, services) =>
            {
                services.AddSingleton(optionsBuilder.Options.Context);
            });
        }
    }
}