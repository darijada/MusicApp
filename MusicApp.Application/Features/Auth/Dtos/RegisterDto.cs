namespace MusicApp.Application.Features.Auth.Dtos;

/// <summary>
/// DTO for user registration request
/// </summary>
public class RegisterDto
{
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}

