using RestHelpers.Attributes;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace RestHelpers.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        ///     Shaping desired fields to data transfer object enumerable.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var expandoObjectList = new List<ExpandoObject>();
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // put required properties into the expando object
                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyAttribute = propertyInfo.GetCustomAttribute(typeof(DtoRequiredAttribute));

                    if (propertyAttribute != null && !propertyInfoList.Contains(propertyInfo))
                    {
                        propertyInfoList.Add(propertyInfo);
                    }
                }

                // put desired properties into the expando object
                foreach (var field in fields.Split(Constants.QueryStringObjectDelimiter))
                {
                    var fieldName = field.Trim();

                    var propertyInfo = typeof(TSource).GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new KeyNotFoundException($"Property {fieldName} wasn't found on {typeof(TSource)}.");
                    }

                    if (!propertyInfoList.Contains(propertyInfo))
                    {
                        propertyInfoList.Add(propertyInfo);
                    }
                }
            }

            //expando object binding
            foreach (var sourceObject in source)
            {
                var dataObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyName = propertyInfo.Name;
                    var propertyValue = propertyInfo.GetValue(sourceObject);

                    if (!((IDictionary<string, object>)dataObject).ContainsKey(propertyName))
                    {
                        ((IDictionary<string, object>)dataObject).Add(propertyName, propertyValue);
                    }
                }
                expandoObjectList.Add(dataObject);
            }

            return expandoObjectList;
        }
    }
}
