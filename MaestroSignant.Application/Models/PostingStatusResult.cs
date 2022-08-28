namespace MaestroSignant.Application.Models;

public class PostingStatusResult
{
    public string Message { get; set; }
    public bool Success { get; set; }
    public PostingStatus Status { get; set; }
}