namespace SettingsUi.Models.ConfigurationScheme.Tokens
{
    public class DecimalScheme : IConfigurationTokenScheme
    {
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
    }
}