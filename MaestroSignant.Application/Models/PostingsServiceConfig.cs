namespace MaestroSignant.Application.Models;

public class PostingsServiceConfig
{
    public static string Position => nameof(PostingsServiceConfig);

    public string DistributorId { get; set; } = string.Empty;
    public string AccessCode { get; set; } = string.Empty;
}