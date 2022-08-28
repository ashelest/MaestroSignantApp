using MaestroSignant.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MaestroSignant.Infrastructure.Extensions;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var databaseService = scope.ServiceProvider.GetRequiredService<Database>();

        databaseService.CreateDatabase();

        return host;
    }
}