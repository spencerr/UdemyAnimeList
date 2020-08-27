using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UdemyAnimeList.Data.Models;

namespace UdemyAnimeList.Data.Models.EntityConfiguration
{
    public class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> builder)
        {
            builder.HasMany(e => e.Animes)
                .WithOne(e => e.Season)
                .HasForeignKey(e => e.SeasonId);
        }
    }
}
