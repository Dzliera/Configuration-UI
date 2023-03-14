using ConfigurationUi.Server.Api.Controllers;
using ConfigurationUi.Server.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace ConfigurationUi.Server.Extensions;

/// <summary>
/// Extensions for <see cref="IHostBuilder"/>
/// </summary>
public static class HostBuilderExtensions
{
    public static IHostBuilder AddConfigurationUi<TSchemaType>(this IHostBuilder hostBuilder, 
        string filePath = "_data/configuration.json", 
        string configApiPath = "/api/configuration-ui/configuration")
    {

        IFileProvider? configurationFileProvider = null;

        CreateFileIfNotExists(filePath);
            
        hostBuilder.ConfigureAppConfiguration((_, builder) =>
        {
            configurationFileProvider = builder.GetFileProvider();
            builder.Sources.Add(new JsonConfigurationSource { Path = filePath, ReloadOnChange = true });
        });

        hostBuilder.ConfigureServices((_, services) =>
        {
            services.Configure<ConfigurationUiServerOptions>(options =>
            {
                options.ConfigApiPath = configApiPath;
                options.UseJsonFileStorage(filePath, configurationFileProvider!);
                options.UseSchemaFromType<TSchemaType>();
            });
            services.AddControllers().AddApplicationPart(typeof(ConfigurationController).Assembly);
        });

        return hostBuilder;
    }

    private static void CreateFileIfNotExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath ?? throw new ArgumentException());
            var streamWriter = File.CreateText(filePath);
            streamWriter.WriteLine("{}"); // write empty json object initially
            streamWriter.Flush();
            streamWriter.Close();
        }
    }
}