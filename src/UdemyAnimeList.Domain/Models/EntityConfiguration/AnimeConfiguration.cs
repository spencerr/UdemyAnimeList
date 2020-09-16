using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAnimeList.Domain.Models;

namespace UdemyAnimeList.Domain.Models.EntityConfiguration
{
    public class AnimeConfiguration : IEntityTypeConfiguration<Anime>
    {
        public void Configure(EntityTypeBuilder<Anime> builder)
        {
            builder.Property(e => e.JapaneseName)
                .IsUnicode(true);

            builder.HasMany(e => e.Episodes)
                .WithOne(e => e.Anime)
                .HasForeignKey(e => e.AnimeId);
        }
    }
}
