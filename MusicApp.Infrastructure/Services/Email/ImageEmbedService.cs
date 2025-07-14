using MusicApp.Application.Features.Email.Services;

public class ImageEmbedService : IImageEmbedService
{
    public string GetImageAsBase64(string relativePath)
    {
        string fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Logo image not found", fullPath);

        byte[] imageBytes = File.ReadAllBytes(fullPath);
        string base64 = Convert.ToBase64String(imageBytes);

        string mime = Path.GetExtension(fullPath).ToLower() switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };

        return $"data:{mime};base64,{base64}";
    }
}
