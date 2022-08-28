using MaestroSignant.Domain;

namespace MaestroSignant.Application.Services;

public interface IPostingsSyncService
{
    Task<IEnumerable<SyncJobInfo>> GetActiveSyncJobs();
}