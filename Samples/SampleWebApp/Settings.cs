using System;

namespace SampleWebApp
{
    [Serializable]
    public class Settings
    {
        public string StringSetting { get; set; }
        public int IntegerSetting { get; set; }
        public bool BooleanSetting { get; set; }
    }
}