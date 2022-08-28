namespace MaestroSignant.Application.Commands;

public class AddPostingCommand
{
    public Guid PostingId { get; set; }
    public Guid AttachmentId { get; set; }
    public Guid PersonId { get; set; }
    public string ContentType { get; set; }
    public string AttachmentName { get; set; }
    public byte[] AttachmentData { get; set; }
}