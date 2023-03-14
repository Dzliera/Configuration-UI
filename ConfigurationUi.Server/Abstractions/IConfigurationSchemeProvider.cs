using NJsonSchema;

namespace ConfigurationUi.Server.Abstractions;

/// <summary>
/// Configuration scheme provider
/// </summary>
public interface IConfigurationSchemeProvider
{
    /// <summary>
    /// Gets configuration scheme
    /// </summary>
    /// <returns></returns>
    public JsonSchema4 GetConfigurationScheme();
}