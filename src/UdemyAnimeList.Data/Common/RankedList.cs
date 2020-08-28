using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyAnimeList.Data.Common
{
    public interface IRankable
    {
        int Rank { get; set; }
    }

    public static class RankedListExtention
    {
        public static IEnumerable<T> AsRankedList<T>(this IEnumerable<T> query, int offset = 0) where T : IRankable
        {
            int rank = offset + 1;
            foreach (var item in query)
            {
                item.Rank = rank++;
                yield return item;
            }
        }
    }
}
