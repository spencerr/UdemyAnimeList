using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyAnimeList.Web.Middleware
{
    public static class DatabaseMigrationExtension
    {

        public static async Task<IHost> MigrateAsync<TDbContext>(this IHost host) where TDbContext : DbContext
        {
            using var scope = host.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<TDbContext>();
            await db.Database.MigrateAsync();

            return host;
        }

        public static IHost Migrate<TDbContext>(this IHost host) where TDbContext : DbContext
        {
            using var scope = host.Services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<TDbContext>();
            db.Database.Migrate();

            return host;
        }

    }
}
