using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ConfigurationUi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NJsonSchema;

namespace ConfigurationUi.Middlewares
{
    internal class ConfigurationUiMiddleware : IMiddleware
    {
        private readonly ConfigurationUiOptions _configurationUiOptions;

        public ConfigurationUiMiddleware(ConfigurationUiOptions configurationUiOptions)
        {
            _configurationUiOptions = configurationUiOptions;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            
            var requestPath = context.Request.Path;
            if (requestPath != _configurationUiOptions.WebUiPath)
            {
                await next(context);
                return;
            }

            var configuration = await _configurationUiOptions.Storage.ReadConfigurationAsync();
            var schema = _configurationUiOptions.Schema;

            var page = GenerateHtml(configuration, schema);

            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(page);
        }
        
        [SuppressMessage("ReSharper", "UnusedParameter.Local")]
        private string GenerateHtml(IConfiguration configuration, JsonSchema schema)
        {
            throw new System.NotImplementedException();
        }
    }
}