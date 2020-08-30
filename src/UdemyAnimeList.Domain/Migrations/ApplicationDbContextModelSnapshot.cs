﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using UdemyAnimeList.Domain;

namespace UdemyAnimeList.Domain.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("UdemyAnimeList.Domain.Models.Anime", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Background")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("BroadcastTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTimeOffset?>("EndAirDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EnglishName")
                        .HasColumnType("text");

                    b.Property<int?>("EpisodeCount")
                        .HasColumnType("integer");

                    b.Property<string>("JapaneseName")
                        .HasColumnType("text")
                        .IsUnicode(true);

                    b.Property<Guid?>("SeasonId")
                        .HasColumnType("uuid");

                    b.Property<int>("ShowType")
                        .HasColumnType("integer");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("StartAirDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Synopsys")
                        .HasColumnType("text");

                    b.Property<int>("TVRating")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("Animes");
                });

            modelBuilder.Entity("UdemyAnimeList.Domain.Models.Configuration", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.ToTable("Configuration");
                });

            modelBuilder.Entity("UdemyAnimeList.Domain.Models.Episode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnimeId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTimeOffset?>("DateAired")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("interval");

                    b.Property<string>("EnglishName")
                        .HasColumnType("text");

                    b.Property<string>("JapaneseName")
                        .HasColumnType("text")
                        .IsUnicode(true);

                    b.Property<short>("Number")
                        .HasColumnType("smallint");

                    b.Property<string>("Synopsys")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AnimeId");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("UdemyAnimeList.Domain.Models.Season", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AiringSeason")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<short>("Year")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("UdemyAnimeList.Domain.Models.Anime", b =>
                {
                    b.HasOne("UdemyAnimeList.Domain.Models.Season", "Season")
                        .WithMany("Animes")
                        .HasForeignKey("SeasonId");
                });

            modelBuilder.Entity("UdemyAnimeList.Domain.Models.Episode", b =>
                {
                    b.HasOne("UdemyAnimeList.Domain.Models.Anime", "Anime")
                        .WithMany("Episodes")
                        .HasForeignKey("AnimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}