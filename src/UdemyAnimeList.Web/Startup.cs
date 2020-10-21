using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UdemyAnimeList.Domain;
using UdemyAnimeList.Web.Infrastructure;
using UdemyAnimeList.Services.Cache;
using Serilog;
using UdemyAnimeList.Web.Middleware;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using UdemyAnimeList.Web.Intrastructure;
using Microsoft.Extensions.Options;
using UdemyAnimeList.Services.Storage;

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
            services.AddControllers(opt => {
                opt.Filters.Add<TransactionFilter>();
                opt.Filters.Add<ValidatorActionFilter>();
            }).AddFluentValidation()
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CdnUrlResolver(Configuration.GetSection("S3Configuration").GetValue<string>("CdnUrl"));
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "client-app/dist";
            });

            services.AddRazorPages()
                .AddFeatureFolders()
                .AddAreaFeatureFolders();

            services.AddLogging();

            var assembly = typeof(Startup).Assembly;
            services.AddFluentValidation(new[] { assembly })
                .AddMediatR(assembly)
                .AddAutoMapper(assembly);

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions())
                .AddAWSService<IAmazonS3>();

            services.Configure<AmazonS3Configuration>(Configuration.GetSection("S3Configuration"));

            services.AddScoped<IConfigurationCache, ConfigurationCache>()
                .AddScoped<IBucketStorage, AmazonS3Service>()
                .AddMemoryCache();

            services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("DefaultConnection")))
                .AddHangfireServer();

            services.AddDbContextPool<ApplicationDbContext>(opt => opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddSwaggerDocument(cfg =>
            {
                cfg.SchemaNameGenerator = new MediatrSchemaNameGenerator();
            });

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
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

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                var tokens = antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });

                await next();
            });

            app.UseHangfireDashboard();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints =>
            {
                /*endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");*/
                endpoints.MapControllers();
            });

            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "client-app";

                if (env.IsDevelopment())
                {
                    spa.UseVueDevelopmentServer();
                }
            });
        }
    }
}
