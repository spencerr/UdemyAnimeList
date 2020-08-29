using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyAnimeList.Domain.Models
{
    public class Configuration
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
