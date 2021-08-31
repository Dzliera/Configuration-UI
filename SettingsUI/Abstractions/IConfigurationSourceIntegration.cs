using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SettingsUi.Abstractions
{
    public interface IConfigurationSourceIntegration
    {
        public Task<IConfiguration> ReadConfigurationAsync();
        public Task WriteConfigurationAsync(IConfiguration configuration);
    }
}