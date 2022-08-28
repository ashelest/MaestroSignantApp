using MaestroSignant.Application.Commands;
using MaestroSignant.Application.Dto;
using MaestroSignant.Application.Models;

namespace MaestroSignant.Application.Services;

public interface IPostingsService
{
    Task<IEnumerable<PersonPostingDto>> GetAllPersonPostingsAsync();

    Task<PostingStatusResult> GetPostingStatusAsync(Guid postingId, Guid attachmentId);

    Task<ServiceResult<PersonPostingDto>> CreateSignPostingAsync(SignPostingCommand command);

    Task UpdatePostingAsync(UpdatePostingCommand command);

    Task<ServiceResult<PostingAttachment>> DownloadAttachmentAsync(Guid postingId, Guid attachmentId);
    Task DeletePostingAsync(Guid postingId);
}