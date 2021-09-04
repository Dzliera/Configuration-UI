using ConfigurationUi.ConfigurationStorage;
using NJsonSchema;

namespace ConfigurationUi.Options.Builder
{
    public class ConfigurationUiOptionsBuilder : IConfigurationUiStorageOptionsBuilder
    {
        internal ConfigurationUiOptions Options { get; } = new ConfigurationUiOptions();

        public IConfigurationUiStorageOptionsBuilder UseJsonFileStorage(string filePath)
        {
            Options.Context.Storage = new JsonConfigurationStorage(filePath);
            return this;
        }
        
        void IConfigurationUiStorageOptionsBuilder.WithSchemeFromType<T>()
        {
            Options.Context.Schema = JsonSchema.FromType<T>();
        }
    }
}