using MaestroSignant.Api.Extensions;
using MaestroSignant.Api.Models;
using MaestroSignant.Application.Commands;
using MaestroSignant.Application.Dto;
using MaestroSignant.Application.Models;
using MaestroSignant.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MaestroSignant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostingController : ControllerBase
{
    private readonly IPostingsService postingsService;

    public PostingController(IPostingsService postingsService)
    {
        this.postingsService = postingsService;
    }

    [HttpPost("sign")]
    public async Task<ServiceResult<PersonPostingDto>> SignPosting([FromForm] SignPostingModel model)
    {
        var command = Map(model);

        var result = await postingsService.CreateSignPostingAsync(command);

        return result;
    }

    [HttpGet("{postingId}/{attachmentId}/attachment")]
    public async Task<ActionResult> DownloadAttachment(Guid postingId, Guid attachmentId)
    {
        var result = await postingsService.DownloadAttachmentAsync(postingId, attachmentId);

        if (result.IsSuccess && result.Entity != null)
        {
            return File(result.Entity.Data, result.Entity.ContentType, result.Entity.Name);
        }

        return Content(result.Message);
    }


    [HttpGet("{postingId}/{attachmentId}/status")]
    public async Task<PostingStatusResult> GetPostingStatus(Guid postingId, Guid attachmentId)
    {
        return await postingsService.GetPostingStatusAsync(postingId, attachmentId);
    }

    [HttpGet]
    public async Task<IEnumerable<PersonPostingDto>> GetAllPersonPostingsAsync()
    {
        var postings = await postingsService.GetAllPersonPostingsAsync();

        return postings;
    }

    [HttpDelete("{postingId}")]
    public async Task<IActionResult> DeleteAttachment(Guid postingId)
    {
        await postingsService.DeletePostingAsync(postingId);

        return Ok();
    }

    private SignPostingCommand Map(SignPostingModel command)
    {
        return new SignPostingCommand
        {
            RecipientName = command.RecipientName,
            RecipientEmail = command.RecipientEmail,
            RecipientMessage = command.RecipientMessage,

            Attachment = new PostingAttachment
            {
                Name = command.File.FileName,
                ContentType = command.File.ContentType,
                Data = command.File.ToBytes()
            }
        };
    }
}