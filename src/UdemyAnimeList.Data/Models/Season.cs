﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAnimeList.Data.Enums;

namespace UdemyAnimeList.Data.Models
{
    public class Season
    {
        public Guid Id { get; set; }

        public AiringSeason AiringSeason { get; set; }
        public short Year { get; set; }

        public ICollection<Anime> Animes { get; set; }
    }
}
