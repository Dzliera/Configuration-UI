﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConfigurationUi.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;

namespace ConfigurationUi.StorageProviders
{
    internal class JsonStorageProvider : IConfigurationStorageProvider
    {
        private readonly string _filePath;
        public  IConfiguration Configuration { get; set; }

        public JsonStorageProvider(string filePath, IFileProvider fileProvider)
        {
            _filePath = filePath;
            
            var fileInfo = fileProvider.GetFileInfo(_filePath);
            if (!fileInfo.Exists)
            {
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath)) 
                    Directory.CreateDirectory(directoryPath);
                
                var streamWriter = File.CreateText(fileInfo.PhysicalPath);
                streamWriter.WriteLine("{}"); // write empty json object initially
                streamWriter.Flush();
                streamWriter.Close();
            }
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetFileProvider(fileProvider);
            Configuration = configurationBuilder.AddJsonFile(fileInfo.PhysicalPath, false, true).Build();
        }
        
        public async Task WriteConfigurationAsync(IConfiguration configuration, JsonSchema4 schema)
        {
            var jToken = GetJTokenFromConfiguration(configuration, schema);
            await using var streamWriter = new StreamWriter(_filePath);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            jsonWriter.Formatting = Formatting.Indented;
            await jToken.WriteToAsync(jsonWriter);
            await jsonWriter.FlushAsync();
        }

        private JToken GetJTokenFromConfiguration(IConfiguration configuration, JsonSchema4 schema)
        {
            if (schema.IsObject)
            {
                var objectToken = new JObject();
                foreach (var propertyConfig in configuration.GetChildren())
                {
                    var propertyToken = GetJTokenFromConfiguration(propertyConfig, schema.Properties[propertyConfig.Key]);
                    objectToken[propertyConfig.Key] = propertyToken;
                }
                
                return objectToken;
            }
            
            if (schema.IsArray)
            {
                var arrayToken = new JArray();
                var elemSchema = schema.Item;
                    
                foreach (var elemConfig in configuration.GetChildren())
                {
                    var elemToken = GetJTokenFromConfiguration(elemConfig, elemSchema);
                    arrayToken.Add(elemToken);
                }

                return arrayToken;
            }


            if (schema.HasReference) return GetJTokenFromConfiguration(configuration, schema.Reference);

            // handle nullable property represented as OneOf Null | Value
            if (schema.OneOf.Count == 2 && schema.OneOf.First().Type == JsonObjectType.Null)
                return GetJTokenFromConfiguration(configuration, schema.OneOf.Last());

            var section = configuration as IConfigurationSection;
            Debug.Assert(section != null, nameof(section) + " != null");

            var value = section.Value;
            
            if (value == null) return JValue.CreateNull();

            if (schema.Type.HasFlag(JsonObjectType.String))
                return new JValue(value);
            if (schema.Type.HasFlag(JsonObjectType.Boolean))
                return new JValue(bool.Parse(value));
            if (schema.Type.HasFlag(JsonObjectType.Integer))
                return new JValue(long.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
            if (schema.Type.HasFlag(JsonObjectType.Number))
                return new JValue(decimal.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
            throw new NotSupportedException($"unsupported json token type {schema.Type}");
        }

    }
}