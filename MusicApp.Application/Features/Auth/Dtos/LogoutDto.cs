namespace MusicApp.Application.Features.Auth.Dtos;

/// <summary>
/// DTO for user logout request
/// </summary>
public class LogoutDto
{
    public string RefreshToken { get; set; } = null!;
}
