﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
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
        private const string TemplatesFolderPath = "Ui/Html";
        private const string RootTemplateName = "ConfigurationUi.html";
        private const string ComponentsFolderName = "Components";

        private readonly string _assemblyBasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private readonly ConfigurationUiOptions _configurationUiOptions;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly FileSystemWatcher _templateChangeWatcher;

        private string _rootUiHtml;
        private string _numericEditorHtmlComponent;
        private string _textEditorHtmlComponent;
        private string _booleanEditorHtmlComponent;
        private string _objectEditorHtmlComponent;


        public ConfigurationUiMiddleware(ConfigurationUiOptions configurationUiOptions)
        {
            _configurationUiOptions = configurationUiOptions;
            _templateChangeWatcher = new FileSystemWatcher(Path.Combine(_assemblyBasePath, TemplatesFolderPath))
            {
                NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite
            };
            _templateChangeWatcher.Changed += (_, _) => InitializeTemplates();
            _templateChangeWatcher.Created += (_, _) => InitializeTemplates();
            _templateChangeWatcher.IncludeSubdirectories = true;
            _templateChangeWatcher.EnableRaisingEvents = true;
            _templateChangeWatcher.Filter = "*";
            InitializeTemplates();
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

            htmlBuilder.Replace("{CfgProperties}", propertiesBuilder.ToString());

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

            if (schema.Type == JsonObjectType.None)
            {
                if (schema.OneOf.Count == 2 && schema.OneOf.First().Type == JsonObjectType.Null)
                    return GenerateHtmlRecursive(configuration, schema.OneOf.Last());

                if (schema.HasReference) return GenerateHtmlRecursive(configuration, schema.Reference);
            }

            // TODO support arrays
            // TODO support enums
            // TODO support Dictionaries
            throw new NotSupportedException();
        }

        private void InitializeTemplates()
        {
            _rootUiHtml = ReadRootTemplate();

            _numericEditorHtmlComponent = ReadComponentTemplate("NumericEditor");
            _textEditorHtmlComponent = ReadComponentTemplate("TextEditor");
            _booleanEditorHtmlComponent = ReadComponentTemplate("BooleanEditor");
            _objectEditorHtmlComponent = ReadComponentTemplate("ObjectEditor");
        }

        private StringBuilder BuildObjectEditor(IConfigurationSection configuration, JsonSchema schema)
        {
            var objectBuilder = new StringBuilder(_objectEditorHtmlComponent)
                .Replace("{PropertyName}", configuration.Key)
                .Replace("{PropertyId}", configuration.Path);

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
            return File.ReadAllText(Path.Combine(_assemblyBasePath, TemplatesFolderPath, ComponentsFolderName,
                $"{componentName}.html"));
        }

        private string ReadRootTemplate()
        {
            return File.ReadAllText(Path.Combine(_assemblyBasePath, TemplatesFolderPath, RootTemplateName));
        }
    }
}