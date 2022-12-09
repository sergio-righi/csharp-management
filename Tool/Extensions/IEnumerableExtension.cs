using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Tool.Resources;

namespace Tool.Extensions
{
    public static class IEnumerableExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<string> list)
        {
            return list.Select(x => new SelectListItem() { Value = x, Text = x });
        }
    }
}