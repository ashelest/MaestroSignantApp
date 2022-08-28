using Dapper;
using Microsoft.Data.SqlClient;

namespace MaestroSignant.Infrastructure.Persistence;

public class Database
{
    private readonly ApplicationDbContext context;

    public Database(ApplicationDbContext context)
    {
        this.context = context;
    }

    public void CreateDatabase()
    {
        var query = "SELECT * FROM sys.databases WHERE name = @name";

        var parameters = new DynamicParameters();
        parameters.Add("@name", GetDbName());

        using var connection = context.CreateConnection(GetMasterConnection());

        var records = connection.Query(query, parameters);

        if (!records.Any())
        {
            var createDbQuery = $"CREATE DATABASE [{GetDbName()}]";

            connection.Execute(createDbQuery);
        }
    }

    private string GetDbName()
    {
        return new SqlConnectionStringBuilder(this.context.ConnectionString).InitialCatalog;
    }

    private string GetMasterConnection()
    {
        var builder = new SqlConnectionStringBuilder(this.context.ConnectionString)
        {
            InitialCatalog = string.Empty
        };

        return builder.ToString();
    }
}