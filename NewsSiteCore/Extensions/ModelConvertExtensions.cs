using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions
{
    public static class ModelConvertExtensions
    {
        public static string ToJson(this object model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public static T ToModel<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Type ToType(this string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(c => c.GetTypes()).SingleOrDefault(x => x.FullName == typeName);
        }

        public static Type ToTypeFromClassName(this string typeName)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(c => c.GetTypes()).SingleOrDefault(x => x.Name == typeName);
        }

        public static object ExtractModel(this string json, string typeName)
        {
            if (string.IsNullOrEmpty(json) || string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            var type = typeName.ToType();
            if (type == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject(json, type);
        }

        public static (string Content, string typeName) ToDbModel(this object obj)
        {
            if (obj == null)
            {
                return (null, null);
            }

            var typeName = obj.GetType().FullName;
            var json = JsonConvert.SerializeObject(obj);
            return (json, typeName);
        }

        public static void FillValues<TDestination>(this TDestination destination, object source)
            where TDestination : class
        {
            var destinationProperties = destination.GetType().GetProperties();
            var sourceProperties = source.GetType().GetProperties();

            foreach (var destinationProperty in destinationProperties)
            {
                var sourceProperty = sourceProperties.FirstOrDefault(c => c.Name == destinationProperty.Name && c.PropertyType == destinationProperty.PropertyType);
                if (sourceProperty == null)
                {
                    continue;
                }
                var newValue = sourceProperty.GetValue(source);
                destinationProperty.SetValue(destination, newValue);
            }
        }

        public static TDestination ConvertClass<TDestination>(this object source)
            where TDestination : class
        {
            var destination = Activator.CreateInstance<TDestination>();
            destination.FillValues(source);
            return destination;
        }

        public static bool IsAssignableFromGenericType(this Type targetType, Type sourceType)
        {
            var interfaceTypes = sourceType.GetInterfaces();
            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == targetType))
            {
                return true;
            }
            if (sourceType.IsGenericType && sourceType.GetGenericTypeDefinition() == targetType)
                return true;
            var baseType = sourceType.BaseType;
            return baseType != null && IsAssignableFromGenericType(baseType, targetType);
        }
    }
}
