using MaestroSignant.Application.Commands;
using MaestroSignant.Application.Dto;
using MaestroSignant.Application.Models;
using MaestroSignant.Application.Persistence;
using MaestroSignant.Application.Services;
using MaestroSignant.Domain;
using IPostingsService = MaestroSignant.Application.Services.IPostingsService;

namespace MaestroSignant.Infrastructure.Services;

public class PostingsService : IPostingsService
{
    private readonly IPostingsIntegrationService postingsIntegrationService;
    private readonly IPersonRepository personRepository;
    private readonly IPostingRepository postingRepository;
    private readonly IPostingSyncRepository postingSyncRepository;

    public PostingsService(IPostingsIntegrationService postingsIntegrationService,
        IPostingRepository postingRepository,
        IPersonRepository personRepository,
        IPostingSyncRepository postingSyncRepository)
    {
        this.postingsIntegrationService = postingsIntegrationService;
        this.postingRepository = postingRepository;
        this.personRepository = personRepository;
        this.postingSyncRepository = postingSyncRepository;
    }

    public async Task<IEnumerable<PersonPostingDto>> GetAllPersonPostingsAsync()
    {
        var postingsInfo = await postingRepository.GetAllPersonPostingsAsync();

        return postingsInfo.Select(p => new PersonPostingDto
        {
            PostingId = p.PostingId,
            AttachmentId = p.AttachmentId,

            PersonId = p.PersonId,
            PersonName = p.PersonName,
            PersonEmail = p.PersonEmail,
            AttachmentName = p.AttachmentName,
            AttachmentStatus = p.AttachmentStatus,

            CreatedDate = p.CreatedDate,
            ModifiedDate = p.ModifiedDate
        });
    }

    public async Task<PostingStatusResult> GetPostingStatusAsync(Guid postingId, Guid attachmentId)
    {
        var statusResult = await postingsIntegrationService.GetPostingStatusAsync(postingId);

        var syncStatusResult = new PostingStatusResult
        {
            Success = statusResult.Success,
            Status = statusResult.Status,
            Message = statusResult.Message
        };

        if (!statusResult.Success) { return syncStatusResult; }

        if (statusResult.Status == PostingStatus.CompletedPartially)
        {
            var updatePostingCommand = new UpdatePostingCommand
            {
                PostingId = postingId,
                Status = PostingStatus.CompletedPartially,
                AttachmentSignedData = null
            };

            await UpdatePostingAsync(updatePostingCommand);

            await postingSyncRepository.CompletePostingSyncJobAsync(postingId, PostingSyncStatus.CompletedPartially);
        }
        else if (statusResult.Status == PostingStatus.Completed)
        {
            var attachment = await postingsIntegrationService.DownloadAttachmentAsync(postingId, attachmentId);

            if (!attachment.Success)
            {
                return new PostingStatusResult
                {
                    Success = attachment.Success,
                    Message = attachment.Message
                };
            }

            var updatePostingCommand = new UpdatePostingCommand
            {
                PostingId = postingId,
                Status = PostingStatus.Completed,
                AttachmentSignedData = attachment.AttachmentData
            };

            await UpdatePostingAsync(updatePostingCommand);

            await postingSyncRepository.CompletePostingSyncJobAsync(postingId, PostingSyncStatus.Completed);
        }

        return syncStatusResult;
    }

    public async Task<ServiceResult<PersonPostingDto>> CreateSignPostingAsync(SignPostingCommand command)
    {
        var postingExists = await postingRepository.PostingExists(command.RecipientEmail);

        if (postingExists)
        {
            return new ServiceResult<PersonPostingDto>
            {
                Error = "You have already created a Posting for the this user. Only one posting per user is available."
            };
        }

        var signPostingResult = await postingsIntegrationService.CreateSignPostingAsync(command);

        //var signPostingResult = new SignPostingResult
        //{
        //    PostingId = new Guid(),
        //    AttachmentId = new Guid(),
        //    Success = true
        //};

        if (signPostingResult.Success && signPostingResult.PostingId != null && signPostingResult.AttachmentId != null)
        {
            var personId = await personRepository.AddPersonIfNotExistAsync(command.RecipientName, command.RecipientEmail);

            var addPostingCommand = new AddPostingCommand
            {
                PostingId = signPostingResult.PostingId.Value,
                ContentType = command.Attachment.ContentType,
                AttachmentName = command.Attachment.Name,
                AttachmentData = command.Attachment.Data,
                AttachmentId = signPostingResult.AttachmentId.Value,
                PersonId = personId
            };

            await postingRepository.AddPostingAsync(addPostingCommand);

            await postingSyncRepository.AddPostingSyncJobAsync(signPostingResult.PostingId.Value);

            return Map(signPostingResult, command, personId);
        }

        return Map(signPostingResult);
    }

    public async Task UpdatePostingAsync(UpdatePostingCommand command)
    {
        await postingRepository.UpdatePostingAsync(command);
    }

    public async Task<ServiceResult<PostingAttachment>> DownloadAttachmentAsync(Guid postingId, Guid attachmentId)
    {
        var attachment = await postingRepository.GetPostingAttachmentAsync(postingId, attachmentId);

        if (attachment != null)
        {
            return new ServiceResult<PostingAttachment> { Entity = attachment };
        }

        return new ServiceResult<PostingAttachment> { Message = "File isn't ready to download. Please, try later." };
    }

    public async Task DeletePostingAsync(Guid postingId)
    {
        await postingsIntegrationService.DeletePostingAsync(postingId);
    }

    private ServiceResult<PersonPostingDto> Map(SignPostingResult result, SignPostingCommand command, Guid personId)
    {
        return new ServiceResult<PersonPostingDto>
        {
            Message = "Posting was created. As soon as document would be signed, we will notify you.",
            Entity = new PersonPostingDto
            {
                PersonEmail = command.RecipientEmail,
                AttachmentName = command.Attachment.Name,
                AttachmentStatus = AttachmentStatus.Created,
                PersonId = personId,
                PersonName = command.RecipientName,
                PostingId = result.PostingId.Value,
                AttachmentId = result.AttachmentId.Value
            }
        };
    }

    private ServiceResult<PersonPostingDto> Map(SignPostingResult result)
    {
        var serviceResult = new ServiceResult<PersonPostingDto>
        {
            Error = result.Message
        };

        return serviceResult;
    }

}