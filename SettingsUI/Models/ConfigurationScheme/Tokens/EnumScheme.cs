using System.Collections.Generic;

namespace SettingsUi.Models.ConfigurationScheme.Tokens
{
    public class EnumConfigurationScheme : IConfigurationTokenScheme
    {
        public IEnumerable<EnumValueDescriptor> PossibleValues { get; set; }
    }

    public class EnumValueDescriptor
    {
        public string Value { get; set; }
        public string ValueName { get; set; }
        public string ValueDescription { get; set; }
    }
}