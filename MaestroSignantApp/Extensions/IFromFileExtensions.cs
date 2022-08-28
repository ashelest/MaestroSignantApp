namespace MaestroSignant.Api.Extensions;

public static class FromFileExtensions
{
    public static byte[] ToBytes(this IFormFile file)
    {
        using var tmpStream = new MemoryStream();
        file.CopyTo(tmpStream);
        return tmpStream.ToArray();
    }
}