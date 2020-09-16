using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyAnimeList.Domain.Models
{
    public class Episode
    {
        public Guid Id { get; set; }
        public Guid AnimeId { get; set; }

        public string JapaneseName { get; set; }
        public string EnglishName { get; set; }
        public short Number { get; set; }
        public string Synopsys { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTimeOffset? DateAired { get; set; }

        public Anime Anime { get; set; }
    }
}
