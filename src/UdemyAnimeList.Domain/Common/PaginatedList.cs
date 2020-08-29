using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyAnimeList.Domain.Common
{
    public class PaginatedList<T> : List<T>
    {
        public int Index { get; }
        public int TotalPages { get; }

        public bool HasPreviousPage => Index > 1;
        public bool HasNextPage => Index < TotalPages;

        public PaginatedList(List<T> items, int count, int index, int size)
        {
            Index = index;
            TotalPages = (int)Math.Ceiling(count / (double)size);
            AddRange(items);
        }
    }

    public static class PaginatedListExtension
    {
        public static async Task<PaginatedList<T>> CreateAsync<T>(this IQueryable<T> source, int index, int size)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((index - 1) * size).Take(size).ToListAsync();
            return new PaginatedList<T>(items, count, index, size);
        }
    }
}
