using Microsoft.Extensions.Configuration;
using NJsonSchema;

namespace ConfigurationUi.Abstractions
{
    public interface IEditorUiBuilder
    {
        string BuildHtml(IConfiguration configuration, JsonSchema schema);
        string BuildComponentHtml(IConfigurationSection configurationSection, JsonSchema schema);
    }
}