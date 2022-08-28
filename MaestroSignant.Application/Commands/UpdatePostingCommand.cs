using MaestroSignant.Application.Models;

namespace MaestroSignant.Application.Commands;

public class UpdatePostingCommand
{
    public Guid PostingId { get; set; }
    public byte[]? AttachmentSignedData { get; set; }
    public PostingStatus Status { get; set; }
}