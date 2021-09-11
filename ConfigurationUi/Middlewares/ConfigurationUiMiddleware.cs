using System.Linq;
using System.Threading.Tasks;
using ConfigurationUi.Abstractions;
using ConfigurationUi.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ConfigurationUi.Middlewares
{
    internal class ConfigurationUiMiddleware : IMiddleware
    {
        
        private readonly ConfigurationUiOptions _configurationUiOptions;
        private readonly IEditorUiBuilder _uiBuilder;


        public ConfigurationUiMiddleware(ConfigurationUiOptions configurationUiOptions, IEditorUiBuilder uiBuilder)
        {
            _configurationUiOptions = configurationUiOptions;
            _uiBuilder = uiBuilder;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var requestPath = context.Request.Path;
            if (requestPath != _configurationUiOptions.WebUiPath)
            {
                await next(context);
                return;
            }

            if (context.Request.Method == HttpMethods.Post)
            {
                var form = context.Request.Form;
                var newConfiguration = new ConfigurationBuilder().AddInMemoryCollection().Build();
                foreach (var (key, value) in form.Where(kv => kv.Key != "action"))
                {
                    newConfiguration[key] = value;
                }

                await _configurationUiOptions.StorageProvider.WriteConfigurationAsync(newConfiguration, _configurationUiOptions.Schema);
            }

            var configuration = await _configurationUiOptions.StorageProvider.ReadConfigurationAsync();
            var schema = _configurationUiOptions.Schema;

            var page = _uiBuilder.BuildHtml(configuration, schema);

            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(page);
        }
        
    }
}