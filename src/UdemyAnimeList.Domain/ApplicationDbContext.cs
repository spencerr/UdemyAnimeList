using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using UdemyAnimeList.Domain.Models;

namespace UdemyAnimeList.Domain
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Anime> Animes { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Models.Configuration> Configuration { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
