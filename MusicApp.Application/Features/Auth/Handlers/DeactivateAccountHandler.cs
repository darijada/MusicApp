using MediatR;
using Microsoft.AspNetCore.Identity;
using MusicApp.Application.Features.Auth.Commands;
using MusicApp.Application.Features.Auth.Dtos;
using MusicApp.Application.Features.Auth.Exceptions;
using MusicApp.Application.Features.Auth.Services;
using MusicApp.Application.Features.Email.Services;
using MusicApp.Core.Entities.Auth;

public class DeactivateAccountHandler
    : IRequestHandler<DeactivateAccountCommand, AuthResultDto>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailSenderService _emailSenderService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IImageEmbedService _imageEmbedService;

    public DeactivateAccountHandler(
            UserManager<User> userManager,
            IEmailSenderService emailSenderService,
            IEmailTemplateService emailTemplateService,
            IRefreshTokenService refreshTokenService,
            IImageEmbedService imageEmbedService
        )

    {
        _userManager = userManager;
        _emailSenderService = emailSenderService;
        _emailTemplateService = emailTemplateService;
        _refreshTokenService = refreshTokenService;
        _imageEmbedService = imageEmbedService;
    }

    public async Task<AuthResultDto> Handle(DeactivateAccountCommand request, CancellationToken ct)
    {
        User? user = await _userManager.FindByIdAsync(request.UserId.ToString())
                   ?? throw new UserNotFoundException(request.UserId);

        await _refreshTokenService.RevokeAllTokensForUserAsync(user.Id, ct);

        user.IsActive = false;
        IdentityResult userUpdate = await _userManager.UpdateAsync(user);
        if (!userUpdate.Succeeded)
        {
            string errs = string.Join("; ", userUpdate.Errors.Select(e => e.Description));
            throw new UserUpdateFailedException(errs);
        }

        string logoBase64 = _imageEmbedService.GetImageAsBase64("Resources/Images/logo.png");

        string html = _emailTemplateService.GetTemplate("Auth", "AccountDeactivated.html", new()
        {
            { "{{LOGO_IMAGE}}", logoBase64 },
            { "{{CURRENT_YEAR}}", DateTime.UtcNow.Year.ToString() },
            { "{{USERNAME}}", user.UserName! }

        });

        await _emailSenderService.SendEmailAsync(
            user.Email!,
            "MusicApp :: Your account has been deactivated",
            html
        );

        return new AuthResultDto
        {
            Succeeded = true,
            Message = "Successful account deactivation"
        };
    }
}
