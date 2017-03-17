#region Imports
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace CySoft.Utility
{
    public static class ListExtension
    {
        public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            var query = source.Select(selector);
            return query.ToList();
        }
    }
}
