namespace PureBakes.Services;

using PureBakes.Service.Services.Interface;

public class FileService(
    IWebHostEnvironment webHostEnvironment) : IFileService
{
    private readonly string _productRootPath = $@"{Path.DirectorySeparatorChar}images{Path.DirectorySeparatorChar}product{Path.DirectorySeparatorChar}";

    public void RemoveOldImageIfExists(string? productImageUrl)
    {
        if (!string.IsNullOrEmpty(productImageUrl))
        {
            var oldImagePath =
                Path.Combine(webHostEnvironment.WebRootPath, productImageUrl.TrimStart(Path.DirectorySeparatorChar));

            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
        }
    }

    public async Task<string?> AddImageToProduct(Stream imageStream, string fileName)
    {
        // Get the absolute path to the "images" folder
        var imagePath = Path.Combine(webHostEnvironment.WebRootPath, _productRootPath.Trim('/'));

        // Combine with the provided file name
        fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
        var imagePathWithFileName = Path.Combine(imagePath, fileName);

        // Save the image to the specified path
        using (var fileStream = new FileStream(imagePathWithFileName, FileMode.Create))
        {
            await imageStream.CopyToAsync(fileStream);
        }

        return _productRootPath + fileName;
    }
}