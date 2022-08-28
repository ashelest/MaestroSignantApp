using System.Reflection;
using DbUp;
using MaestroSignant.Application.Persistence;
using MaestroSignant.Application.Services;
using MaestroSignant.Infrastructure.Persistence;
using MaestroSignant.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MaestroSignant.Infrastructure;

public static class InfrastructureBootstrap
{
    private const string DefaultConnection = nameof(DefaultConnection);

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPostingsService, PostingsService>();
        services.AddScoped<IPostingsSyncService, PostingsSyncService>();
        services.AddScoped<IPostingsIntegrationService, PostingsIntegrationService>();

        services.AddScoped<IPostingRepository, PostingRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IPostingSyncRepository, PostingSyncRepository>();

        AddPersistence(services, configuration);

        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ApplicationDbContext>(options => 
            new ApplicationDbContext(configuration.GetConnectionString(DefaultConnection)));

        services.AddSingleton<Database>();

        AddSqlMigrations(configuration);
    }

    private static void AddSqlMigrations(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(DefaultConnection);

        EnsureDatabase.For.SqlDatabase(connectionString);

        var migrator = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = migrator.PerformUpgrade();

        if (!result.Successful)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
        }
    }
}