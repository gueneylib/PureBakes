namespace PureBakes.Service.Services.Interface;

public interface IFileService
{
    Task<string?> AddImageToProduct(Stream imageStream, string fileName);
    void RemoveOldImageIfExists(string? productImageUrl);
}