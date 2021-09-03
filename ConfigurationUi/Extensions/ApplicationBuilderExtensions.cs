using ConfigurationUi.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ConfigurationUi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseConfigurationUi(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ConfigurationUiMiddleware>();
        }
    }
}