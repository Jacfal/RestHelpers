using RestHelpers.Attributes;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace RestHelpers.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        ///     Shaping desired fields.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static ExpandoObject ShapeData<TSource>(this TSource source, string fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var dataObject = new ExpandoObject();

            // all public properties should be part of data transfer object if fields doesn't provided
            var properties = typeof(TSource).GetTypeInfo()
                .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            // put all properties into the expando object
            if (string.IsNullOrWhiteSpace(fields))
            {
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(source);

                    ((IDictionary<string, object>)dataObject).Add(property.Name, propertyValue);
                }

                return dataObject;
            }

            // TODO vyresit duplictni field, kdyz ho user zada do fields a je povinny, tak se nachazi 2x v expando objectu

            // put required properties into the expando object
            foreach (var property in properties)
            {
                var propertyAttribute = property.GetCustomAttribute(typeof(DtoRequiredAttribute));

                if (propertyAttribute != null)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(source);

                    if (!((IDictionary<string, object>)dataObject).ContainsKey(propertyName))
                    {
                        ((IDictionary<string, object>)dataObject).Add(propertyName, propertyValue);
                    }
                }
            }

            // put desired properties into the expando object
            foreach (var field in fields.Split(Constants.QueryStringObjectDelimiter))
            {
                var fieldName = field.Trim();

                var property = typeof(TSource).GetTypeInfo()
                    .GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property == null)
                {
                    throw new KeyNotFoundException($"Property {fieldName} wasn't found on {typeof(TSource)}.");
                }

                var propertyName = property.Name;
                var propertyValue = property.GetValue(source);

                if (!((IDictionary<string, object>)dataObject).ContainsKey(propertyName))
                {
                    ((IDictionary<string, object>)dataObject).Add(propertyName, propertyValue);
                }
            }

            return dataObject;
        }

        /// <summary>
        ///     Shaping data for pagination.
        /// </summary>
        /// <typeparam name="TSource">Source object type.</typeparam>
        /// <param name="baseResourceParameters">Base resource parameters for object.</param>
        /// <param name="resourceUriType">Valid resource URI type.</param>
        /// <param name="page">Actual page if collection.</param>
        /// <returns></returns>
        internal static ExpandoObject ShapeData<TSource>(this TSource baseResourceParameters, ResourceUriType resourceUriType, int page)
        {
            if (baseResourceParameters == null)
            {
                throw new ArgumentNullException("baseResourceParameters");
            }

            var dataObject = new ExpandoObject();

            var properties = typeof(TSource).GetTypeInfo()
                .GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(baseResourceParameters);
                var propertyName = property.Name;
                var propertyType = property.GetType();

                if (propertyName == "Page")
                {
                    switch (resourceUriType)
                    {
                        case ResourceUriType.PreviousPage:
                            propertyValue = page - 1;
                            break;
                        
                        case ResourceUriType.NextPage:
                            propertyValue = page + 1;
                            break;
                            
                        case ResourceUriType.Current:
                            break;

                        default:
                            break;
                    }
                }
                
                ((IDictionary<string, object>)dataObject).Add(propertyName, propertyValue);
            }

            return dataObject;
        }
    }
}