using MaestroSignant.Infrastructure.Extensions;

namespace MaestroSignant.Api;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
            .Build()
            .MigrateDatabase()
            .Run();
    }

    //builder.AddJsonFile($"appsettings.{Environment.MachineName}.json", true, true);

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((ctx, builder) =>
            {
                builder.AddJsonFile("appsettings.json", false, true);
                builder.AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}