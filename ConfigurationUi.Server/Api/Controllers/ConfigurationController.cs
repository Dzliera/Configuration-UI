using ConfigurationUi.Server.Api.Models;
using ConfigurationUi.Server.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Options;

namespace ConfigurationUi.Server.Api.Controllers
{
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("/api/configuration-ui/configuration")]
    public class ConfigurationController : ControllerBase
    {
        private readonly ConfigurationUiServerOptions _options;

        public ConfigurationController(IOptions<ConfigurationUiServerOptions> options)
        {
            _options = options.Value;
        }

        [HttpGet("")]
        public GetConfigurationResponse GetConfiguration()
        {
            return new GetConfigurationResponse
            {
                Configuration = _options.StorageProvider.Configuration.AsEnumerable().ToDictionary(x => x.Key, x => x.Value),
                Schema = _options.Schema
            };
        }

        [HttpPut("")]
        public async Task SaveConfiguration([FromBody]Dictionary<string, string> configuration)
        {
            var providers = new List<IConfigurationProvider>
            {
                new MemoryConfigurationProvider(new MemoryConfigurationSource
                {
                    InitialData = configuration
                })
            };
            var configurationRoot = new ConfigurationRoot(providers);
            await _options.StorageProvider.SaveConfigurationAsync(configurationRoot, _options.Schema);
        }
    }
}