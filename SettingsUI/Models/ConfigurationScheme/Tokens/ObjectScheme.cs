using System.Collections.Generic;

namespace SettingsUi.Models.ConfigurationScheme.Tokens
{
    public class ObjectScheme : IConfigurationTokenScheme
    {
        public IEnumerable<PropertyScheme> Properties { get; }

        public ObjectScheme(IEnumerable<PropertyScheme> properties)
        {
            Properties = properties;
        }
    }

    public class PropertyScheme
    {
        public string PropertyName { get; internal set; }
        public bool Required { get; internal set; }
        public IConfigurationTokenScheme ValueScheme { get; internal set; }
    }
}