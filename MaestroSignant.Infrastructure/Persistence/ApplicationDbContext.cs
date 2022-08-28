using Microsoft.Data.SqlClient;

namespace MaestroSignant.Infrastructure.Persistence;

public class ApplicationDbContext
{
    internal readonly string ConnectionString;

    public ApplicationDbContext(string connectionString)
    {
        this.ConnectionString = connectionString;
    }

    public SqlConnection CreateConnection(string connection = null) => new SqlConnection(connection ?? this.ConnectionString);
}