namespace MusicApp.Application.Features.Email.Services;

public interface IImageEmbedService
{
    string GetImageAsBase64(string relativePath);
}
