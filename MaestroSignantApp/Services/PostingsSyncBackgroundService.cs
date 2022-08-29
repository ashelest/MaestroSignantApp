using MaestroSignant.Api.SignalR;
using MaestroSignant.Application.Dto;
using MaestroSignant.Application.Models;
using MaestroSignant.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace MaestroSignant.Api.Services;

public class PostingsSyncBackgroundService : BackgroundService
{
    private readonly IHubContext<SignantHub> hubContext;
    private readonly IServiceProvider services;

    public PostingsSyncBackgroundService(IServiceProvider services, IHubContext<SignantHub> hubContext)
    {
        this.services = services;
        this.hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            using IServiceScope scope = services.CreateScope();

            var postingsService = scope.ServiceProvider.GetRequiredService<IPostingsService>();
            var syncService = scope.ServiceProvider.GetRequiredService<IPostingsSyncService>();

            foreach (var job in await syncService.GetActiveSyncJobs())
            {
                var result = await postingsService.GetPostingStatusAsync(job.PostingId, job.AttachmentId);

                if (PostingWasProcessed(result))
                {
                    string eventName = null;

                    switch (result.Status)
                    {
                        case PostingStatus.Completed:
                            eventName = SignantHubEvents.PostingSigned;
                            break;

                        case PostingStatus.CompletedPartially:
                            eventName = SignantHubEvents.PostingRejected;
                            break;
                    }

                    if (eventName is null) { return; }

                    await hubContext.Clients.All.SendCoreAsync(eventName, new[] { job.PostingId.ToString() });
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private bool PostingWasProcessed(PostingStatusResult result)
    {
        return result.Success && (result.Status == PostingStatus.Completed ||
                                  result.Status == PostingStatus.CompletedPartially);
    }
}