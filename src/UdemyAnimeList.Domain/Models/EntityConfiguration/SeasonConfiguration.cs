using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UdemyAnimeList.Domain.Models;

namespace UdemyAnimeList.Domain.Models.EntityConfiguration
{
    public class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> builder)
        {
            builder.HasMany(e => e.Animes)
                .WithOne(e => e.Season)
                .HasForeignKey(e => e.SeasonId);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now() at time zone 'utc'");
        }
    }
}
