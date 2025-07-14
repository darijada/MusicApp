namespace MusicApp.Application.Features.Auth.Dtos;

/// <summary>
/// DTO for refresh token request
/// </summary>
public class RefreshTokenDto
{
    public string RefreshToken { get; set; } = null!;
}
