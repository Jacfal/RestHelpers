using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using RestHelpers.Attributes;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace RestHelpers
{
    public static class IQueryAbleExtensions
    {
        /// <summary>
        ///     Apply sort for query of objects.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IQueryable<TSource> ApplySort<TSource>(this IQueryable<TSource> source, string orderBy)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var properties = typeof(TSource).GetTypeInfo()
                .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            var propertyNameList = new List<string>();
            
            // get properties with "OrderByProperty" attribute
            foreach (var property in properties)
            {
                var propertyAttribute = property.GetCustomAttribute(typeof(OrderByPropertyAttribute));

                if (propertyAttribute != null)
                {
                    propertyNameList.Add(property.Name.ToLowerInvariant());
                }
            }

            if (propertyNameList == default(List<string>))
            {
                // there is no property with "OrderByProperty" attribute
                return source;
            }

            var orderByEnum = orderBy.Split(Constants.QueryStringObjectDelimiter);

            foreach (var orderByField in orderByEnum.Reverse())
            {
                var orderByClause = orderByField.Trim()
                    .ToLowerInvariant();

                var isDescending = orderByClause.EndsWith($" {Constants.DescendingClause}");

                // delete ordering clause
                var indexOffFirstSpace = orderByClause.IndexOf(" ");
                var orderByPropertyName = indexOffFirstSpace == -1 ?
                    orderByClause : orderByClause.Remove(indexOffFirstSpace);

                if (!propertyNameList.Contains(orderByPropertyName))
                {
                    throw new KeyNotFoundException($"Data cann't be ordered by {orderByPropertyName}.");
                }

                source = source.OrderBy(orderByPropertyName + (isDescending ? " descending" : " ascending"));
            }

            return source;
        }

        public static IQueryable<TSource> Search<TSource>(this IQueryable<TSource> source, IEnumerable<PropertyInfo> propertyInfo)
        {
            var sourcePropertyinfo = typeof(TSource).GetTypeInfo()
                .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in propertyInfo)
            {
                if (sourcePropertyinfo.Contains(property))
                {
                    return source;
                }
            }

            return source;
        }
    }
}
