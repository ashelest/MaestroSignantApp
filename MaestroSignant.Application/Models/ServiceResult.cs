namespace MaestroSignant.Application.Models;

public class ServiceResult<T>
{
    public string Error { get; set; } = string.Empty;
    
    public string Message { get; set; } = string.Empty;

    public bool IsSuccess => string.IsNullOrWhiteSpace(Error);

    public T Entity { get; set; }
}