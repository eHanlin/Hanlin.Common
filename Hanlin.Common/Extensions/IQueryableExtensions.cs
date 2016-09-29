using System;
using System.Linq;
using System.Linq.Expressions;

namespace Hanlin.Common.Extensions
{
    /// <summary>
    /// Reference: http://stackoverflow.com/a/22227975/494297
    /// </summary>
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string sortField, bool ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, sortField);
            var exp = Expression.Lambda(prop, param);
            string method = ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}