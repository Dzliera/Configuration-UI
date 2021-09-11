using ConfigurationUi.StorageProviders;
using Microsoft.Extensions.FileProviders;
using NJsonSchema;

namespace ConfigurationUi.Options.Builder
{
    public class ConfigurationUiOptionsBuilder : IConfigurationUiStorageOptionsBuilder
    {
        internal ConfigurationUiOptions Options { get; } = new ConfigurationUiOptions();

        public IConfigurationUiStorageOptionsBuilder UseJsonFileStorage(string filePath, IFileProvider fileProvider)
        {
            Options.StorageProvider = new JsonStorageProvider(filePath, fileProvider);
            return this;
        }
        
        void IConfigurationUiStorageOptionsBuilder.WithSchemeFromType<T>()
        {
            Options.Schema = JsonSchema.FromType<T>();
        }
    }
}