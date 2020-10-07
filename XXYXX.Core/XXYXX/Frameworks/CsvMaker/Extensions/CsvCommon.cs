#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsvMaker.Attributes;

namespace CsvMaker.Extensions
{
    public static class CsvCommon
    {
        public static IEnumerable<PropertyInfo> GetPropertiesInOrder(this Type myType, List<string>? ignoredPropertyNames = null)
        {
            var properties = myType.GetProperties().Where(x => x.GetCustomAttribute<CsvMakerPropertyIgnoreAttribute>() == null).OrderBy(x => x.GetCustomAttribute<CsvMakerColumnOrderAttribute>() != null ? x.GetCustomAttribute<CsvMakerColumnOrderAttribute>().Order : 100);
            
            if (ignoredPropertyNames == null) return properties;
            else return properties.Where(x => !ignoredPropertyNames.Contains(x.Name));
        }
    }
}
