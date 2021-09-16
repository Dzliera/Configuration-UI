using ConfigurationUi.Abstractions;
using ConfigurationUi.Middlewares;
using ConfigurationUi.Options.Builder;
using ConfigurationUi.Ui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConfigurationUi.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddConfigurationUi<TConfigModel>(this IHostBuilder hostBuilder, string filePath)
        {
            var optionsBuilder = new ConfigurationUiOptionsBuilder();
            
            
            hostBuilder.ConfigureAppConfiguration((_, builder) =>
            {
                optionsBuilder.UseJsonFileStorage(filePath, builder.GetFileProvider()).WithSchemeFromType<TConfigModel>();
                builder.Sources.Add(new JsonConfigurationSource() { Path = filePath });
            });

            hostBuilder.ConfigureServices((_, services) =>
            {
                services.AddSingleton(optionsBuilder.Options);
                services.AddSingleton<IEditorUiBuilder, UiBuilder>();
                services.AddSingleton<ConfigurationUiMiddleware>();
            });

            return hostBuilder;
        }
    }
}