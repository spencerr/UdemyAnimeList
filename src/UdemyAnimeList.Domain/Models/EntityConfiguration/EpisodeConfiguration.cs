using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using UdemyAnimeList.Domain.Models;

namespace UdemyAnimeList.Domain.Models.EntityConfiguration
{
    public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
    {
        public void Configure(EntityTypeBuilder<Episode> builder)
        {
            builder.Property(e => e.JapaneseName)
                .IsUnicode(true);
        }
    }
}
