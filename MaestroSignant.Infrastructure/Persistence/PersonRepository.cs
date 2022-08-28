using Dapper;
using MaestroSignant.Application.Persistence;
using MaestroSignant.Domain;
using Microsoft.Data.SqlClient;

namespace MaestroSignant.Infrastructure.Persistence;

public class PersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext context;

    public PersonRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Guid> AddPersonIfNotExistAsync(string name, string email)
    {
        await using var connection = context.CreateConnection();

        var person = new
        {
            PersonId = Guid.NewGuid(),
            Name = name,
            Email = email,
            Phone = string.Empty,
            CreatedDate = DateTime.UtcNow
        };

        var result = await connection.QueryAsync<Guid>(Queries.InsertPersonIfNotExist, person);

        return result.First();
    }
}