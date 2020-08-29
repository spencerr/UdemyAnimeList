using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyAnimeList.Domain.Common
{
    public interface IRankable
    {
        int Rank { get; set; }
    }

    public static class RankedListExtention
    {
        public static IEnumerable<T> ToRankedList<T>(this IEnumerable<T> query, int offset = 0) where T : IRankable
        {
            int rank = offset + 1;
            foreach (var item in query)
            {
                item.Rank = rank++;
                yield return item;
            }
        }

        public static async Task<IEnumerable<T>> ToRankedListAsync<T>(this IQueryable<T> query, int offset = 0) where T : class, IRankable
        {
            var rank = offset + 1;
            var result = await query.ToListAsync();
            foreach (var item in result)
            {
                item.Rank = rank++;
            }

            return result;
        }
    }
}
