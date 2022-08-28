using MaestroSignant.Application.Commands;
using MaestroSignant.Application.Models;
using MaestroSignant.Domain;

namespace MaestroSignant.Application.Persistence;

public interface IPostingRepository
{
    Task<IEnumerable<PersonPosting>> GetAllPersonPostingsAsync();

    Task<bool> PostingExists(string recipientEmail);

    Task<Guid> AddPostingAsync(AddPostingCommand command);

    Task<Guid> UpdatePostingAsync(UpdatePostingCommand command);

    Task<PostingAttachment> GetPostingAttachmentAsync(Guid postingId, Guid attachmentId);
}