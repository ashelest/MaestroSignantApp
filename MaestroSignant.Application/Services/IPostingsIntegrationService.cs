using MaestroSignant.Application.Commands;
using MaestroSignant.Application.Models;

namespace MaestroSignant.Application.Services;

public interface IPostingsIntegrationService
{
    Task<SignPostingResult> CreateSignPostingAsync(SignPostingCommand command);

    Task<PostingStatusResult> GetPostingStatusAsync(Guid postingId);

    Task<DownloadAttachmentResult> DownloadAttachmentAsync(Guid postingId, Guid attachmentId);

    Task DeletePostingAsync(Guid postingId);
}