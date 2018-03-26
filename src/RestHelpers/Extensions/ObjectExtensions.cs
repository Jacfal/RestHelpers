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
        ///     This extension is used to fields data filtering from source object.
        /// </summary>
        /// <typeparam name="TSource">Object type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="fields">Fields separated by certain separator.</param>
        /// <returns></returns>
        public static ExpandoObject ShapeData<TSource>(this TSource source, string fields)
        {
            var splitFields = fields.Split(Constants.QueryStringObjectDelimiter);

            return source.ShapeData(splitFields);
        }

        /// <summary>
        ///     This extension is used to fields data filtering from source object.
        /// </summary>
        /// <typeparam name="TSource">Object type.</typeparam>
        /// <param name="source">Source object.</param>
        /// <param name="fields">Fields which should be part of the returned object.</param>
        /// <returns>Filtered object.</returns>
        public static ExpandoObject ShapeData<TSource>(this TSource source, params string[] fields)
        {
            if (source == null)
            {
                throw new ArgumentNullException("Source cann't be null.");
            }

            var dataObject = new ExpandoObject();

            // all public properties should be part of data transfer object if fields doesn't provided
            var properties = typeof(TSource).GetTypeInfo()
                .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            // fileds arary is empty - all fields will be part of returned object
            if (fields.Length < 1)
            {
                foreach (var property in properties)
                {
                    var propertyValue = property.GetValue(source);

                    ((IDictionary<string, object>)dataObject).Add(property.Name, propertyValue);
                }

                return dataObject;
            }

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
            foreach (var field in fields)
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