using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Tool.Resources;
using Tool.Utilities;

namespace Tool.Extensions
{
    public static class IQueryableExtension
    {
        public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> list, Expression<Func<T, TKey>> expression, EDirection direction)
        {
            return direction == EDirection.Ascending ? list.OrderBy(expression) : list.OrderByDescending(expression);
        }
    }
}
