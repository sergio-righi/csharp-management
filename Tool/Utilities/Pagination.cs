
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tool.Utilities
{
    public sealed class Pagination<T> : List<T>
    {
        /// <summary>
        /// page index
        /// </summary>
        public int Page { get; private set; }
        /// <summary>
        /// number of pages
        /// </summary>
        public int Quantity { get; private set; }
        /// <summary>
        /// number of items on each page
        /// </summary>
        public int Show { get; private set; }
        /// <summary>
        /// number of rows on database
        /// </summary>
        public int Total { get; private set; }

        public Pagination(IEnumerable<T> items, int total, int page, int show)
        {
            this.Show = show;
            this.Page = page;
            this.Total = total;
            this.Quantity = (int)Math.Ceiling(total / (double)show);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (Page > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (Page < Quantity);
            }
        }

        public static async Task<Pagination<T>> CreateAsync(IQueryable<T> source, int total, int page, int show)
        {
            return new Pagination<T>(source, total, page, show);
        }
    }
}
