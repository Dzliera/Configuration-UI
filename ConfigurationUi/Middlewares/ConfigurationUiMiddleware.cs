using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConfigurationUi.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NJsonSchema;

namespace ConfigurationUi.Middlewares
{
    internal class ConfigurationUiMiddleware : IMiddleware
    {
        private const string ComponentBasePath = "Ui/Html/Components";
        private const string RootTemplatePath = "Ui/Html/ConfigurationUi.html";
        
        private readonly ConfigurationUiOptions _configurationUiOptions;
        
        private readonly string _rootUiHtml;
        private readonly string _numericEditorHtmlComponent;
        private readonly string _textEditorHtmlComponent;
        private readonly string _booleanEditorHtmlComponent;
        private readonly string _objectEditorHtmlComponent;
        

        public ConfigurationUiMiddleware(ConfigurationUiOptions configurationUiOptions)
        {
            _configurationUiOptions = configurationUiOptions;

            _rootUiHtml = ReadRootTemplate();
            
            _numericEditorHtmlComponent = ReadComponentTemplate("NumericEditor");
            _textEditorHtmlComponent = ReadComponentTemplate("TextEditor");
            _booleanEditorHtmlComponent = ReadComponentTemplate("BooleanEditor");
            _objectEditorHtmlComponent = ReadComponentTemplate("ObjectEditor");
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
            var htmlBuilder = new StringBuilder(_rootUiHtml);

            Debug.Assert(schema.Type == JsonObjectType.Object, "schema.Type == JsonObjectType.Object");

            var propertiesBuilder = GeneratePropertiesHtmlRecursive(configuration, schema);

            htmlBuilder.Replace("{cfgProperties}", propertiesBuilder.ToString());

            return htmlBuilder.ToString();
        }

        private StringBuilder GenerateHtmlRecursive(IConfigurationSection configuration, JsonSchema schema)
        {
            if (schema.Type.HasFlag(JsonObjectType.String))
                return BuildTextEditorHtml(configuration);
            if (schema.Type.HasFlag(JsonObjectType.Boolean))
                return BuildBooleanEditor(configuration);
            if (schema.Type.HasFlag(JsonObjectType.Number))
                return BuildNumberEditor(configuration);
            if (schema.Type.HasFlag(JsonObjectType.Integer)) return BuildNumberEditor(configuration);

            if (schema.IsObject) return BuildObjectEditor(configuration, schema);

            // TODO support arrays
            throw new NotSupportedException();
        }

        private StringBuilder BuildObjectEditor(IConfigurationSection configuration, JsonSchema schema)
        {
            var objectBuilder =  new StringBuilder(_objectEditorHtmlComponent)
                .Replace("{PropertyName}", configuration.Key);

            var propertiesBuilder = GeneratePropertiesHtmlRecursive(configuration, schema);
            objectBuilder.Replace("{Properties}", propertiesBuilder.ToString());

            return objectBuilder;
        }

        private StringBuilder BuildTextEditorHtml(IConfigurationSection configuration)
        {
            return FormatComponent(configuration, _textEditorHtmlComponent);
        }
        
        private StringBuilder BuildBooleanEditor(IConfigurationSection configuration)
        {
            return FormatComponent(configuration, _booleanEditorHtmlComponent);
        }
        
        private StringBuilder BuildNumberEditor(IConfigurationSection configuration)
        {
            return FormatComponent(configuration, _numericEditorHtmlComponent);
        }



        private StringBuilder FormatComponent(IConfigurationSection configuration, string template)
        {
            return new StringBuilder(template)
                .Replace("{PropertyName}", configuration.Key)
                .Replace("{PropertyId}", configuration.Path)
                .Replace("{Value}", configuration.Value);
        }
        
        
        private StringBuilder GeneratePropertiesHtmlRecursive(IConfiguration configuration, JsonSchema schema)
        {
            var propertiesBuilder = new StringBuilder();
            foreach (var jsonSchemaProperty in schema.Properties.Values)
            {
                var propertyConfiguration = configuration.GetSection(jsonSchemaProperty.Name);
                var propertyHtml = GenerateHtmlRecursive(propertyConfiguration, jsonSchemaProperty);
                propertiesBuilder.Append(propertyHtml);
            }

            return propertiesBuilder;
        }


        private string ReadComponentTemplate(string componentName)
        {
            
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(Path.Combine(basePath!, ComponentBasePath, $"{componentName}.html"));
        }

        private string ReadRootTemplate()
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return File.ReadAllText(Path.Combine(basePath!, RootTemplatePath));
        }
    }
}