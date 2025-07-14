using FluentValidation;
using MusicApp.Application.Features.Auth.Dtos;

namespace MusicApp.Application.Features.Auth.Validators;

/// <summary>
/// Validator for LogoutDto.
/// </summary>
public class LogoutDtoValidator : AbstractValidator<LogoutDto>
{
    public LogoutDtoValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required for logout.");
    }
}
