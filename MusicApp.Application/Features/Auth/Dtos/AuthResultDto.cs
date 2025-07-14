namespace MusicApp.Application.Features.Auth.Dtos;

/// <summary>
/// DTO that returns a message and optionally a JWT token after an authentication operation
/// </summary>
public class AuthResultDto
{
    public bool Succeeded { get; set; }
    public string Message { get; set; } = null!;
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
