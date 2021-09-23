using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NJsonSchema;

namespace ConfigurationUi.Abstractions
{
    public interface IConfigurationStorageProvider
    {
        public IConfiguration Configuration { get; set; }
        public Task WriteConfigurationAsync(IConfiguration configuration, JsonSchema4 schema);
    }
}