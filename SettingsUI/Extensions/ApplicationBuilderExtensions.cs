using Microsoft.AspNetCore.Builder;
using SettingsUi.Middlewares;

namespace SettingsUi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseConfigurationUi(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ConfigurationUiMiddleware>();
        }
    }
}