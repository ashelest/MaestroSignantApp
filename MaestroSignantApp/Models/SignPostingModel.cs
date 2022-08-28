namespace MaestroSignant.Api.Models;

public class SignPostingModel
{
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientEmail { get; set; } = string.Empty;
    public string RecipientMessage { get; set; } = string.Empty;

    public IFormFile File { get; set; }
}