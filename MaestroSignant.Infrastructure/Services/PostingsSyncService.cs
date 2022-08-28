using MaestroSignant.Application.Persistence;
using MaestroSignant.Application.Services;
using MaestroSignant.Domain;

namespace MaestroSignant.Infrastructure.Services;

public class PostingsSyncService : IPostingsSyncService
{
    private readonly IPostingSyncRepository repository;

    public PostingsSyncService(IPostingSyncRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IEnumerable<SyncJobInfo>> GetActiveSyncJobs()
    {
        return await repository.GetPostingSyncJobsAsync();
    }
}