using MaestroSignant.Application.Models;

namespace MaestroSignant.Application.Commands;

public class SignPostingCommand
{
    public string RecipientName { get; set; } = string.Empty;
    public string RecipientEmail { get; set; } = string.Empty;
    public string RecipientMessage { get; set; } = string.Empty;

    public PostingAttachment Attachment { get; set; }

    public bool NotifyByEmail { get; set; } = true;
    public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddMonths(1);
}