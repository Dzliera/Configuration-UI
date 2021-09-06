using ConfigurationUi.ConfigurationStorage;
using Microsoft.Extensions.FileProviders;
using NJsonSchema;

namespace ConfigurationUi.Options.Builder
{
    public class ConfigurationUiOptionsBuilder : IConfigurationUiStorageOptionsBuilder
    {
        internal ConfigurationUiOptions Options { get; } = new ConfigurationUiOptions();

        public IConfigurationUiStorageOptionsBuilder UseJsonFileStorage(string filePath, IFileProvider fileProvider)
        {
            Options.Storage = new JsonConfigurationStorage(filePath, fileProvider);
            return this;
        }
        
        void IConfigurationUiStorageOptionsBuilder.WithSchemeFromType<T>()
        {
            Options.Schema = JsonSchema.FromType<T>();
        }
    }
}