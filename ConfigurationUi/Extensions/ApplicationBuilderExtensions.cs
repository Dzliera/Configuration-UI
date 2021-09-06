using ConfigurationUi.Middlewares;
using ConfigurationUi.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationUi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseConfigurationUi(this IApplicationBuilder builder, string webUiPath)
        {
            builder.ApplicationServices.GetRequiredService<ConfigurationUiOptions>().WebUiPath = webUiPath;
            builder.UseMiddleware<ConfigurationUiMiddleware>();
        }
    }
}