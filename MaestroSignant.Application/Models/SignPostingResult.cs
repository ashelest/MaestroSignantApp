namespace MaestroSignant.Application.Models;

public class SignPostingResult
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public Guid? PostingId { get; set; }
    public Guid? AttachmentId { get; set; }

}