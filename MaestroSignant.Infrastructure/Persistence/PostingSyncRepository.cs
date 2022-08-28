using Dapper;
using MaestroSignant.Application.Persistence;
using MaestroSignant.Domain;

namespace MaestroSignant.Infrastructure.Persistence;

public class PostingSyncRepository : IPostingSyncRepository
{
    private readonly ApplicationDbContext context;

    public PostingSyncRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Guid> AddPostingSyncJobAsync(Guid postingId)
    {
        await using var connection = context.CreateConnection();

        var jobParams = new
        {
            Id = Guid.NewGuid(),
            SyncStatus = PostingSyncStatus.New,
            PostingId = postingId,
            CreatedDate = DateTime.UtcNow
        };

        await connection.ExecuteAsync(Queries.InsertPostingSyncJob, jobParams);

        return jobParams.Id;
    }

    public async Task CompletePostingSyncJobAsync(Guid postingId, PostingSyncStatus status)
    {
        await using var connection = context.CreateConnection();

        var jobParams = new
        {
            PostingId = postingId,
            SyncStatus = status,
            ModifiedDate = DateTime.UtcNow
        };

        await connection.ExecuteAsync(Queries.CompletePostingSyncJob, jobParams);
    }

    public async Task UpdatePostingSyncJobAsync(Guid jobId, PostingSyncStatus status)
    {
        await using var connection = context.CreateConnection();

        var jobParams = new
        {
            Id = jobId,
            SyncStatus = status,
            ModifiedDate = DateTime.UtcNow
        };

        await connection.ExecuteAsync(Queries.UpdatePostingSyncJob, jobParams);
    }

    public async Task<IEnumerable<SyncJobInfo>> GetPostingSyncJobsAsync()
    {
        await using var connection = context.CreateConnection();

        var jobParams = new
        {
            SyncStatus = PostingSyncStatus.New
        };

        return await connection.QueryAsync<SyncJobInfo>(Queries.GetPostingSyncJobs, jobParams);
    }
}