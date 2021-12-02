using FM21.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FM21.Data
{
    public static class Extension
    {
        public static async Task<PagedEntityResponse<T>> GetPaged<T>(this IQueryable<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedEntityResponse<T> { CurrentPage = page, PageSize = pageSize, RowCount = query.Count() };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            if(query is IAsyncEnumerable<T>)
            {
                result.Data = await query.Skip(skip).Take(pageSize).ToListAsync();
            }
            else
            {
                result.Data = query.Skip(skip).Take(pageSize).ToList();
            }
            return result;
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, bool ascending)
        {
            return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }

        public static PagedEntityResponse<T> GetPaged<T>(this ICollection<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedEntityResponse<T> { CurrentPage = page, PageSize = pageSize, RowCount = query.Count() };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Data = query.Skip(skip).Take(pageSize).ToList();
            return result;
        }

        public static PagedEntityResponse<T> GetPaged<T>(this IList<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedEntityResponse<T> { CurrentPage = page, PageSize = pageSize, RowCount = query.Count() };

            var pageCount = (double)result.RowCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;
            result.Data = query.Skip(skip).Take(pageSize).ToList();
            return result;
        }

        public static void SetColumnsOrder(this DataTable tbl, params string[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                tbl.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }
    }
}