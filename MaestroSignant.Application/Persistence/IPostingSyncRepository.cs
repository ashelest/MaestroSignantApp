using MaestroSignant.Domain;

namespace MaestroSignant.Application.Persistence;

public interface IPostingSyncRepository
{
    Task<Guid> AddPostingSyncJobAsync(Guid postingId);

    Task CompletePostingSyncJobAsync(Guid postingId, PostingSyncStatus status);

    Task<IEnumerable<SyncJobInfo>> GetPostingSyncJobsAsync();
}