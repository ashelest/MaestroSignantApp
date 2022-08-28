namespace MaestroSignant.Application.Persistence;

public interface IPersonRepository
{
    Task<Guid> AddPersonIfNotExistAsync(string name, string email);
}