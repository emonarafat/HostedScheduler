using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using WorkerService1.Helpers;

using static WorkerService1.Helpers.Constants;

namespace WorkerService1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
          .ReadFrom.Configuration(GetConfig(args))
          .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }
        private static IConfiguration GetConfig(string[] args) =>
    new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        //.AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();

        private static string EnvironmentName =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<AppConfig>(options => hostContext.Configuration.GetSection("AppConfig").Bind(options))
                    .AddSingleton<IDbConnection>(f => new SqlConnection(hostContext.Configuration.GetConnectionString(DEFAULT_DATA_BASE_CONNECTION)))
                    .AddHostedService<Worker>();
                });
    }
}
