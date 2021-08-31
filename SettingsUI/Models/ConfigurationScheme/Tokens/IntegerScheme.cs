namespace SettingsUi.Models.ConfigurationScheme.Tokens
{
    public class IntegerScheme : IConfigurationTokenScheme
    {
        public long MinValue { get; set; }
        public long MaxValue { get; set; }
    }
}