namespace BlazorSample.Server;

[Serializable]
public class Settings
{
    public string? StringSetting { get; set; }
    public int IntegerSetting { get; set; }
    public bool BooleanSetting { get; set; }
    public SettingsSubSection? SettingsSubSection { get; set; }
}

[Serializable]
public class SettingsSubSection
{
    public MyEnum EnumSetting { get; set; }
    public decimal DecimalSetting { get; set; }
    public string[]? ArraySetting { get; set; }
}

[Serializable]
public enum MyEnum { FirstValue, SecondValue }