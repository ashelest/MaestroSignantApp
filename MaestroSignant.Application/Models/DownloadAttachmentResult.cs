namespace MaestroSignant.Application.Models;

public class DownloadAttachmentResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public byte[]? AttachmentData { get; set; }
}