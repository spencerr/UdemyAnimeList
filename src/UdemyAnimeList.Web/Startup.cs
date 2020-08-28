using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using Grinderofl.FeatureFolders;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UdemyAnimeList.Data;
using UdemyAnimeList.Web.Infrastructure;
using UdemyAnimeList.Web.Intrastructure.Services;

namespace UdemyAnimeList.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(opt =>
            {
                opt.Filters.Add<TransactionFilter>();
                opt.Filters.Add<ValidatorActionFilter>();
            });

            services.AddOptions<AzureFileStorage>("AzureFileStorage");

            services.AddRazorPages()
                .AddFeatureFolders()
                .AddAreaFeatureFolders();

            var assembly = typeof(Startup).Assembly;
            services.AddFluentValidation(new[] { assembly })
                .AddMediatR(assembly)
                .AddAutoMapper(assembly);

            services.AddSingleton<IAzureFileStorage, AzureFileStorage>()
                .AddScoped<IConfigurationCache, ConfigurationCache>();

            services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("DefaultConnection")))
                .AddHangfireServer();

            services.AddDbContextPool<ApplicationDbContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")))
                .AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
