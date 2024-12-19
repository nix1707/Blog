
using PhotoSauce.MagicScaler;

namespace Blog.Data.FileManager;

public class FileManager(IConfiguration config, ILogger<FileManager> logger) : IFileManager
{
    private readonly string _imagePath = config["Path:Images"]!;
    private readonly ILogger<FileManager> _logger = logger;

    private static ProcessImageSettings ImageSettings => new()
    {
        Width = 800,
        Height = 500,
        ResizeMode = CropScaleMode.Crop,
    };

    public FileStream ImageStream(string image)
        => new(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);

    public bool RemoveImage(string image)
    {
        try
        {
            var file = Path.Combine(_imagePath, image);

            if (File.Exists(file))
                File.Delete(file);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to remove image: {ex}", ex);
            return false;
        }
    }

    public async Task<string> SaveImageAsync(IFormFile image)
    {
        try
        {
            var savePath = Path.Combine(_imagePath);

            if (Directory.Exists(savePath) == false)
                Directory.CreateDirectory(savePath);

            var mime = image.FileName[image.FileName.LastIndexOf('.')..];
            var fileName = $"img_{DateTime.Now:dd-MM-yyyy-HH-mm-ss}{mime}";

            await Task.Run(() =>
            {
                using var fileStream = new FileStream(Path.Combine(savePath, fileName), FileMode.Create);
                MagicImageProcessor.ProcessImage(image.OpenReadStream(), fileStream, ImageSettings);
            });

            return fileName;

        }
        catch (Exception ex)
        {
            _logger.LogError("Error with saving image: {ex}", ex);
            throw;

        }
    }
}
