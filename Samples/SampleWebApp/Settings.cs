using System;

namespace SampleWebApp
{
    [Serializable]
    public class Settings
    {
        public string StringSetting { get; set; }
        public int IntegerSetting { get; set; }
        public bool BooleanSetting { get; set; }
        public SettingSubSection SettingsSubSection { get; set; }
    }

    [Serializable]
    public class SettingSubSection
    {
        public string StringSetting { get; set; }
        public int IntegerSetting { get; set; }
        public bool BooleanSetting { get; set; }
    }
}