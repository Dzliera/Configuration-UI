using SettingsUi.Models.ConfigurationScheme;

namespace SettingsUi.Abstractions
{
    public interface IConfigurationSchemeProvider
    {
        public ConfigurationScheme GetConfigurationScheme();
    }

}