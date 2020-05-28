using System;
using System.Collections;
using System.Linq;

namespace AutocorApi.Extensions
{
    public static class ModelExtension
    {
        /// <summary>
        /// Serializa un objeto en un query string
        /// </summary>
        /// <param name="model">El objeto a serializar</param>
        /// <param name="separator">Separador para items IEnumerable</param>
        /// <returns></returns>
        public static string ToQueryString(this object model, string separator = ",")
        {
            if (model == null)
                throw new ArgumentNullException("request");

            // Get all properties on the object
            var properties = model.GetType().GetProperties()
                .Where(x => x.CanRead)
                .Where(x => x.GetValue(model, null) != null)
                .ToDictionary(x => x.Name, x => x.GetValue(model, null));

            // Get names for all IEnumerable properties (excl. string)
            var propertyNames = properties
                .Where(x => !(x.Value is string) && x.Value is IEnumerable)
                .Select(x => x.Key)
                .ToList();

            // Concat all IEnumerable properties into a comma separated string
            foreach (var key in propertyNames)
            {
                var valueType = properties[key].GetType();
                var valueElemType = valueType.IsGenericType
                                        ? valueType.GetGenericArguments()[0]
                                        : valueType.GetElementType();

                if (valueElemType.IsPrimitive || valueElemType == typeof(string))
                {
                    var enumerable = properties[key] as IEnumerable;
                    properties[key] = string.Join(separator, enumerable.Cast<object>());
                }
            }

            // Concat all key/value pairs into a string separated by ampersand
            return string.Join("&", properties
                .Select(x => string.Concat(
                    Uri.EscapeDataString(x.Key), "=",
                    Uri.EscapeDataString(x.Value.ToString()))));
        }
    }
}