namespace MaestroSignant.Application.Models;

public class PostingAdmin
{
    public static string Position => nameof(PostingAdmin);

    public string SSN { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool NotifyByEmail { get; set; }
    public string MobileNumber { get; set; }
}