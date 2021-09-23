using Microsoft.Extensions.Configuration;
using NJsonSchema;

namespace ConfigurationUi.Abstractions
{
    public interface IEditorUiBuilder
    {
        string BuildHtml(IConfiguration configuration, JsonSchema4 schema);
        string BuildComponentHtml(IConfigurationSection configurationSection, JsonSchema4 schema);
    }
}