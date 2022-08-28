using MaestroPostingsService;
using MaestroSignant.Application.Commands;
using MaestroSignant.Application.Models;
using MaestroSignant.Application.Services;
using Microsoft.Extensions.Options;
using PostingAdmin = MaestroPostingsService.PostingAdmin;

namespace MaestroSignant.Infrastructure.Services;

public class PostingsIntegrationService : IPostingsIntegrationService
{
    private readonly PostingsServiceConfig config;
    private readonly PostingAdmin postingAdmin;

    public PostingsIntegrationService(IOptions<PostingsServiceConfig> config, IOptions<Application.Models.PostingAdmin> postingAdmin)
    {
        this.config = config.Value;

        this.postingAdmin = new PostingAdmin
        {
            Name = postingAdmin.Value.Name,
            Email = postingAdmin.Value.Email,
            MobileNumber = postingAdmin.Value.MobileNumber,
            NotifyByEmail = postingAdmin.Value.NotifyByEmail,
            SSN = postingAdmin.Value.SSN
        };
    }

    public async Task<SignPostingResult> CreateSignPostingAsync(SignPostingCommand command)
    {
        var client = new PostingsServiceClient();

        var post = new Posting
        {
            Title = command.RecipientMessage,
            
            ActiveTo = command.ExpirationDate,
            WillBeDeletedDateTime = command.ExpirationDate.AddDays(1),

            UseWidget = true,
            AutoActivate = true,

            PostingAdmins = new[] { postingAdmin },
            Recipients = new[] { GetRecipient(command) },
            Attachments = new[] { GetAttachment(command) },
        };

        var signPosting = await client.CreateSignPostingAsync(config.DistributorId, config.AccessCode, post);

        client.Close();

        return Map(signPosting);
    }

    public async Task<PostingStatusResult> GetPostingStatusAsync(Guid postingId)
    {
        var client = new PostingsServiceClient();

        var status = await client.GetPostingStatusAsync(config.DistributorId, config.AccessCode, postingId);

        client.Close();

        return new PostingStatusResult
        {
            Success = status.Success,
            Message = status.Message,
            Status = (PostingStatus)status.Status
        };
    }

    public async Task DeletePostingAsync(Guid postingId)
    {
        var client = new PostingsServiceClient();

        var result = await client.DeletePostingAsync(config.DistributorId, config.AccessCode, postingId, string.Empty);

        client.Close();
    }

    public async Task<DownloadAttachmentResult> DownloadAttachmentAsync(Guid postingId, Guid attachmentId)
    {
        var client = new PostingsServiceClient();

        var attachment = await client.DownloadAttachmentAsync(config.DistributorId, config.AccessCode, postingId, attachmentId, string.Empty);

        client.Close();

        return new DownloadAttachmentResult
        {
            Success = attachment.Success,
            Message = attachment.Message,
            AttachmentData = attachment.AttachmentFile,
        };
    }

    private SignPostingResult Map(CreateSignPostingResponse signPosting)
    {
        if (signPosting.Success)
        {
            return new SignPostingResult
            {
                PostingId = signPosting.PostingID,
                AttachmentId = signPosting.AttachmentInfos.First().AttachmentID,

                Success = signPosting.Success,
                Message = signPosting.Message
            };
        }

        return new SignPostingResult
        {
            Success = signPosting.Success,
            Message = signPosting.Message
        };
    }

    private Recipient GetRecipient(SignPostingCommand command)
    {
        return new Recipient
        {
            Name = command.RecipientName,
            Email = command.RecipientEmail,
            NotifyByEmail = command.NotifyByEmail
        };
    }

    private Attachment GetAttachment(SignPostingCommand command)
    {
        return new Attachment
        {
            ActionType = ActionType.Sign,

            FileName = command.Attachment.Name,
            Description = command.Attachment.Name,

            File = command.Attachment.Data,
        };
    }
}