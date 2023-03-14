using Microsoft.Extensions.Configuration;
using NJsonSchema;

namespace ConfigurationUi.Server.Abstractions;

/// <summary>
/// Responsible for storing and loading configuration
/// </summary>
public interface IConfigurationStorageProvider
{
    /// <summary>
    /// Current Configuration
    /// </summary>
    public IConfiguration Configuration { get; set; }
        
    /// <summary>
    /// Saves configuration
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="schema"></param>
    public Task SaveConfigurationAsync(IConfiguration configuration, JsonSchema4 schema);
}