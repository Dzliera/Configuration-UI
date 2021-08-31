namespace SettingsUi.Models.ConfigurationScheme.Tokens
{
    public class ArrayScheme : IConfigurationTokenScheme
    {
        public IConfigurationTokenScheme ElemScheme { get; }
        public ArrayScheme(IConfigurationTokenScheme elemScheme)
        {
            ElemScheme = elemScheme;
        }
    }
    
    
}