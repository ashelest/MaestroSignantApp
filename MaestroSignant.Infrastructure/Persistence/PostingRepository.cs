using Dapper;
using MaestroSignant.Application.Commands;
using MaestroSignant.Application.Models;
using MaestroSignant.Application.Persistence;
using MaestroSignant.Domain;

namespace MaestroSignant.Infrastructure.Persistence;

public class PostingRepository : IPostingRepository
{
    private readonly ApplicationDbContext context;

    public PostingRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<PersonPosting>> GetAllPersonPostingsAsync()
    {
        await using var connection = context.CreateConnection();

        return await connection.QueryAsync<PersonPosting>(Queries.GetPersonPostings);
    }

    public async Task<bool> PostingExists(string recipientEmail)
    {
        await using var connection = context.CreateConnection();

        var queryParams = new
        {
            Email = recipientEmail
        };

        var result = await connection.QueryAsync<bool>(Queries.PostingExists, queryParams);

        return result.FirstOrDefault();
    }

    public async Task<Guid> AddPostingAsync(AddPostingCommand command)
    {
        await using var connection = context.CreateConnection();

        var addPostingParams = new
        {
            PostingId = command.PostingId,
            AttachmentId = command.AttachmentId,
            PersonId = command.PersonId,
            ContentType = command.ContentType,
            AttachmentName = command.AttachmentName,
            AttachmentOriginalData = command.AttachmentData,
            Status = AttachmentStatus.Created,
            CreatedDate = DateTime.UtcNow,
        };

        await connection.ExecuteAsync(Queries.InsertPosting, addPostingParams);

        return command.PostingId;
    }

    public async Task<Guid> UpdatePostingAsync(UpdatePostingCommand command)
    {
        await using var connection = context.CreateConnection();

        var postingParams = new
        {
            Status = command.Status,
            AttachmentSignedData = command.AttachmentSignedData,
            ModifiedDate = DateTime.UtcNow,
            PostingId = command.PostingId
        };

        await connection.ExecuteAsync(Queries.UpdatePosting, postingParams);

        return command.PostingId;
    }

    public async Task<PostingAttachment> GetPostingAttachmentAsync(Guid postingId, Guid attachmentId)
    {
        await using var connection = context.CreateConnection();

        var attachmentParams = new
        {
            PostingId = postingId,
            AttachmentId = attachmentId
        };

        var result = await connection.QueryAsync<PostingAttachment>(Queries.GetAttachment, attachmentParams);

        return result.FirstOrDefault();
    }
}