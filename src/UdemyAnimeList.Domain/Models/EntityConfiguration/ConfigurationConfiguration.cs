using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyAnimeList.Domain.Models.EntityConfiguration
{
    public class ConfigurationConfiguration : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            builder.HasKey(e => e.Key);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now() at time zone 'utc'");
        }
    }
}
