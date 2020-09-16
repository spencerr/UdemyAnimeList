using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAnimeList.Domain.Enums;

namespace UdemyAnimeList.Domain.Models
{
    public partial class Anime
    {
        public Guid Id { get; set; }
        public Guid? SeasonId { get; set; }

        public string JapaneseName { get; set; }
        public string EnglishName { get; set; }
        public string Synopsys { get; set; }
        public string Background { get; set; }
        public string Source { get; set; }
        public int? EpisodeCount { get; set; }
        public string ImageUrl { get; set; }

        public DateTimeOffset? StartAirDate { get; set; }
        public DateTimeOffset? EndAirDate { get; set; }
        public DateTimeOffset? BroadcastTime { get; set; }

        public ShowType ShowType { get; set; }
        public TVRating TVRating { get; set; }

        public Season Season { get; set; }
        public ICollection<Episode> Episodes { get; set; }

    }
}
