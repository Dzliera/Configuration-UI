using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NJsonSchema;

namespace ConfigurationUi.Abstractions
{
    public interface IConfigurationStorage
    {
        public Task<IConfiguration> ReadConfigurationAsync();
        public Task WriteConfigurationAsync(IConfiguration configuration, JsonSchema schema);
    }
}