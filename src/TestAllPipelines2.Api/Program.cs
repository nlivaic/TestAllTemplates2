using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using SparkRoseDigital.Infrastructure.Logging;

namespace TestAllPipelines2.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SparkRoseDigital.Infrastructure.Logging.LoggerExtensions.ConfigureSerilogLogger("ASPNETCORE_ENVIRONMENT");

            try
            {
                Log.Information("Starting up TestAllPipelines2.");
                CreateHostBuilder(args)
                    .Build()
                    .AddW3CTraceContextActivityLogging()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "TestAllPipelines2 failed at startup.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}
