using ConfigurationUi.Server.Abstractions;
using ConfigurationUi.Server.StorageProviders.JsonStorageProvider;
using Microsoft.Extensions.FileProviders;
using NJsonSchema;

namespace ConfigurationUi.Server.Options
{
    public class ConfigurationUiServerOptions
    {
        public IConfigurationStorageProvider StorageProvider { get; set; } = null!;
        public JsonSchema4 Schema { get; set; } = null!;

        public string ConfigApiPath { get; set; } = null!;

        
        /// <summary>
        /// Sets json file path, where configuration will be stored
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileProvider"></param>
        /// <returns></returns>
        public ConfigurationUiServerOptions UseJsonFileStorage(string filePath, IFileProvider fileProvider)
        {
            StorageProvider = new JsonStorageProvider(filePath, fileProvider);
            return this;
        }

        /// <summary>
        /// Reflects configuration schema from CRL type
        /// </summary>
        /// <typeparam name="TConfigModel"></typeparam>
        /// <returns></returns>
        public ConfigurationUiServerOptions UseSchemaFromType<TConfigModel>()
        {
            Schema = JsonSchema4.FromTypeAsync<TConfigModel>().Result;
            return this;
        }
    }
}