namespace MaestroSignant.Application.Models;

public class PostingAttachment
{
    public string Name { get; set; }
    public string ContentType { get; set; }
    public byte[] Data { get; set; }
}