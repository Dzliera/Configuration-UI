using SettingsUi.Abstractions;

namespace SettingsUI.Sources.Json.Options
{
    public class JsonSourceOptions
    {
        public string FilePath { get; set; }
        public IConfigurationSchemeProvider SchemeProvider { get; set; }
    }
}