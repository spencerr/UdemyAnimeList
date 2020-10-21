using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using UdemyAnimeList.Domain;
using UdemyAnimeList.Web.Middleware;

namespace UdemyAnimeList.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();

                await host.MigrateAsync<ApplicationDbContext>();
                await host.RunAsync();
            }
            catch(Exception ex)
            {
                if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .CreateLogger();
                }

                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddSystemsManager($"/UdemyAnimeList/{context.HostingEnvironment.EnvironmentName}/", optional: context.HostingEnvironment.IsDevelopment());
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .CaptureStartupErrors(true)
                        .UseSerilog((hostingContext, loggerConfiguration) =>
                        {
                            loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                                .Enrich.FromLogContext()
                                .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                                .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);
#if DEBUG
                            loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
                        });
                });
    }
}
