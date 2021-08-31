using System;
using System.Collections.Generic;
using System.Linq;
using SettingsUi.Abstractions;
using SettingsUi.Extensions;
using SettingsUi.Models.ConfigurationScheme;
using SettingsUi.Models.ConfigurationScheme.Tokens;

namespace SettingsUi.SchemeProviders
{
    public class TypeSchemeProvider<TSourceType> : IConfigurationSchemeProvider
    {
        private readonly Type[] _integerTypes =
        {
            typeof(long), typeof(ulong),
            typeof(int), typeof(uint),
            typeof(short), typeof(ushort),
            typeof(byte), typeof(sbyte),
        };

        private readonly Type[] _decimalTypes =
        {
            typeof(decimal), typeof(float), typeof(double)
        };


        public ConfigurationScheme GetConfigurationScheme()
        {
            var type = typeof(TSourceType);
            var scheme = new ConfigurationScheme
            {
                RootToken = BuildConfigurationScheme(type)
            };

            return scheme;
        }

        private IConfigurationTokenScheme BuildConfigurationScheme(Type currentType)
        {
            if (CheckAndProcessBuiltInTypes(currentType, out var buildConfigurationScheme)) return buildConfigurationScheme;

            if (CheckAndProcessArray(currentType, out var arrayScheme)) return arrayScheme;
            
            // if we came here current type should be complex object, so we can iterate on its properties recursively
            return ProcessComplexType(currentType);
        }

        private IConfigurationTokenScheme ProcessComplexType(Type currentType)
        {
            var properties = new List<PropertyScheme>();

            foreach (var propertyInfo in currentType.GetProperties())
            {
                var propertyType = propertyInfo.PropertyType;
                var scheme = BuildConfigurationScheme(propertyType);
                properties.Add(new PropertyScheme
                {
                    PropertyName = propertyInfo.Name,
                    Required = true,
                    ValueScheme = scheme
                });
            }

            return new ObjectScheme(properties);
        }

        private bool CheckAndProcessArray(Type currentType, out IConfigurationTokenScheme arrayScheme)
        {
            if (currentType.IsGenericEnumerable(out var elemType))
            {
                var elemScheme = BuildConfigurationScheme(elemType);
                {
                    arrayScheme = new ArrayScheme(elemScheme);
                    return true;
                }
            }

            arrayScheme = null;
            return false;
        }

        private bool CheckAndProcessBuiltInTypes(Type currentType, out IConfigurationTokenScheme buildConfigurationScheme)
        {
            if (currentType == typeof(string))
            {
                {
                    buildConfigurationScheme = new StringScheme();
                    return true;
                }
            }

            if (_integerTypes.Contains(currentType))
            {
                {
                    buildConfigurationScheme = new IntegerScheme();
                    return true;
                }
            }

            if (_decimalTypes.Contains(currentType))
            {
                {
                    buildConfigurationScheme = new DecimalScheme();
                    return true;
                }
            }

            if (currentType == typeof(DateTime))
            {
                {
                    buildConfigurationScheme = new DateTimeScheme();
                    return true;
                }
            }

            if (currentType == typeof(TimeSpan))
            {
                {
                    buildConfigurationScheme = new DateTimeScheme();
                    return true;
                }
            }

            buildConfigurationScheme = null;
            return false;
        }
    }
}