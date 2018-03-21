using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RestHelpers
{
    /// <summary>
    ///     List with paging implementation.
    /// </summary>
    /// <typeparam name="T">dsd</typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        ///     List with paging implementation.
        /// </summary>
        /// <param name="items">Items fetched to actual page.</param>
        /// <param name="count">Total amount of items.</param>
        /// <param name="pageNumber">Actual page number.</param>
        /// <param name="pageSize">Actual page size.</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        /// <summary>
        ///     Create new paged list collection.
        /// </summary>
        /// <param name="source">Source data.</param>
        /// <param name="pageNumber">Actual page number.</param>
        /// <param name="pageSize">Size of page.</param>
        /// <returns>Paged list collection.</returns>
        public async static Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var allItems = source.Count();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PagedList<T>(items, allItems, pageNumber, pageSize);
        }

        /// <summary>
        ///     Current page.
        /// </summary>
        public int CurrentPage { get; private set; }
        /// <summary>
        ///     Number of total pages.
        /// </summary>
        public int TotalPages { get; private set; }
        /// <summary>
        ///     Size of page.
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        ///     Total amount of items.
        /// </summary>
        public int TotalCount { get; private set; }     // total ammount of items

        /// <summary>
        ///     Determine if previous page exists.
        /// </summary>
        public bool HasPrevious
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        /// <summary>
        ///     Determine if next page exists.
        /// </summary>
        public bool HasNext
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
    }
}
