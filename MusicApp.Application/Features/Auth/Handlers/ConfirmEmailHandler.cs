using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Features.Auth.Commands;
using MusicApp.Application.Features.Auth.Dtos;
using MusicApp.Application.Features.Auth.Exceptions;
using MusicApp.Application.Features.Email.Services;
using MusicApp.Core.Entities.Auth;
using System.Text;

public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, AuthResultDto>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailSenderService _emailSender;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IImageEmbedService _imageEmbedService;

    public ConfirmEmailHandler(
        UserManager<User> userManager, 
        IEmailSenderService emailSender,
        IEmailTemplateService emailTemplateService,
        IImageEmbedService imageEmbedService)
    {
        _userManager = userManager;
        _emailSender = emailSender;
        _emailTemplateService = emailTemplateService;
        _imageEmbedService = imageEmbedService;
    }

    public async Task<AuthResultDto> Handle(ConfirmEmailCommand request, CancellationToken ct)
    {
        User? user = await _userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user is null)
            throw new UserNotFoundException(request.UserId);

        if (user.EmailConfirmed)
            throw new EmailAlreadyConfirmedException();

        string token = DecodeToken(request.Code);

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            throw new EmailConfirmationFailedException();

        user.IsActive = true;

        await _userManager.UpdateAsync(user);

        string logoBase64 = _imageEmbedService.GetImageAsBase64("Resources/Images/logo.png");

        string html = _emailTemplateService.GetTemplate("Auth", "WelcomeEmail.html", new()
        {
            { "{{LOGO_IMAGE}}", logoBase64 },
            { "{{CURRENT_YEAR}}", DateTime.UtcNow.Year.ToString() },
            { "{{USERNAME}}", user.UserName! }
        }); 


        await _emailSender.SendEmailAsync(
            user.Email!,
            "MusicApp :: Welcome!",
            html
        );

        return new AuthResultDto
        {
            Succeeded = true,
            Message = "Successful email confirmation"
        };
    }

    private string DecodeToken(string encoded)
    {
        try
        {
            var bytes = WebEncoders.Base64UrlDecode(encoded);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            throw new InvalidConfirmationCodeException();
        }
    }

}
