using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigurationUi.Abstractions;
using ConfigurationUi.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NJsonSchema;

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

            var request = context.Request;


            IConfiguration configuration;
            
            if (request.Method == HttpMethods.Post)
            {
                var newConfiguration = new ConfigurationBuilder().AddInMemoryCollection().Build();
                foreach (var (key, value) in request.Form.Where(kv => kv.Key != "action"))
                {
                    newConfiguration[key] = value;
                }

                await _configurationUiOptions.StorageProvider.WriteConfigurationAsync(newConfiguration,
                    _configurationUiOptions.Schema);

                configuration = newConfiguration;
            } else configuration = _configurationUiOptions.StorageProvider.Configuration;
            
            
            var schema = _configurationUiOptions.Schema;

            string html;

            if (request.Query.TryGetValue("configPath", out var configPath))
            {
                var configSection = GetSectionByConfigPath(configuration, configPath, request);

                var componentSchema = GetSchemaByConfigSection(configSection, schema);

                html = _uiBuilder.BuildComponentHtml(configSection, componentSchema);
            }
            else html = _uiBuilder.BuildHtml(configuration, schema);


            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(html);
        }

        private static IConfigurationSection GetSectionByConfigPath(IConfiguration configuration, StringValues configPath,
            HttpRequest request)
        {
            var configSection = configuration.GetSection(configPath);


            if (request.Query.TryGetValue("type", out var componentType))
            {
                if (componentType == "newArrayItem")
                {
                    var itemIndex = request.Query["itemIndex"];
                    configSection = configSection.GetSection(itemIndex);
                    // ReSharper restore PossibleMultipleEnumeration
                }
            }

            return configSection;
        }

        private JsonSchema4 GetSchemaByConfigSection(IConfigurationSection configSection, JsonSchema4 rootSchema)
        {
            var path = configSection.Path;
            var sectionsToTraverse = path.Split(":");
            return GetSchemaByConfigPathRecursive(rootSchema, new Queue<string>(sectionsToTraverse));
        }

        private JsonSchema4 GetSchemaByConfigPathRecursive(JsonSchema4 currentSchema,
            Queue<string> sectionsToTraverse)
        {
            if (currentSchema.HasReference)
                return GetSchemaByConfigPathRecursive(currentSchema.Reference, sectionsToTraverse);
            if (currentSchema.OneOf.Count == 2 && currentSchema.OneOf.First().Type == JsonObjectType.Null)
                return GetSchemaByConfigPathRecursive(currentSchema.OneOf.Last(), sectionsToTraverse);


            if (sectionsToTraverse.Count == 0)
            {
                return currentSchema;
            }

            var sectionKey = sectionsToTraverse.Dequeue();


            if (uint.TryParse(sectionKey, out _))
            {
                // if configuration key can be parsed as uint, we have array here
                return GetSchemaByConfigPathRecursive(currentSchema.Item, sectionsToTraverse);
            }

            return GetSchemaByConfigPathRecursive(currentSchema.Properties[sectionKey], sectionsToTraverse);
        }
    }
}