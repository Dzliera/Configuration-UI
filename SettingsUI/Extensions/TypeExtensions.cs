using System;
using System.Collections.Generic;

namespace SettingsUi.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsGenericEnumerable(this Type type, out Type elemType)
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                if(!interfaceType.IsGenericType) continue;
                if(interfaceType.GetGenericTypeDefinition() != typeof(IEnumerable<>)) continue;

                var typeArgument = interfaceType.GenericTypeArguments[0];

                elemType = typeArgument;

                return true;
            }
            
            elemType = null;
            return false;
        }
    }
}