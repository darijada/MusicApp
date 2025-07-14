namespace MusicApp.Application.Features.Auth.Dtos;

/// <summary>
/// DTO for user login request
/// </summary>
public class LoginDto
{
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
}