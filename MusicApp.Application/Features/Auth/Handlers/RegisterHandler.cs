using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using MusicApp.Application.Features.Auth.Commands;
using MusicApp.Application.Features.Auth.Dtos;
using MusicApp.Application.Features.Auth.Exceptions;
using MusicApp.Application.Features.Email.Services;
using MusicApp.Core.Entities.Auth;
using System.Text;

namespace MusicApp.Application.Features.Auth.Handlers;

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResultDto>
{
    private readonly UserManager<User> _userManager;
    private readonly IEmailSenderService _emailSenderService;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IImageEmbedService _imageEmbedService;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public RegisterHandler(
        UserManager<User> userManager,
        IEmailSenderService emailSenderService,
        IEmailTemplateService emailTemplateService,
        IImageEmbedService imageEmbedService,
        IConfiguration configuration,
        IMapper mapper)
    {
        _userManager = userManager;
        _emailSenderService = emailSenderService;
        _emailTemplateService = emailTemplateService;
        _imageEmbedService = imageEmbedService;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<AuthResultDto> Handle(RegisterCommand request, CancellationToken ct)
    {
        RegisterDto dto = request.Dto;
        User user = _mapper.Map<User>(dto);

        IdentityResult createResult = await _userManager.CreateAsync(user, dto.Password);
        if (!createResult.Succeeded)
        {
            string errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
            return new AuthResultDto { Succeeded = false, Message = errors };
        }

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        string confirmLink = BuildEmailConfirmationLink(user.Id, encodedToken);

        string logoBase64 = _imageEmbedService.GetImageAsBase64("Resources/Images/logo.png");

        string html = _emailTemplateService.GetTemplate("Auth", "ConfirmEmail.html", new()
        {
            { "{{CONFIRM_LINK}}", confirmLink },
            { "{{LOGO_IMAGE}}", logoBase64 },
            { "{{CURRENT_YEAR}}", DateTime.UtcNow.Year.ToString() },
            { "{{USERNAME}}", user.UserName! }
        });

        if (!string.IsNullOrWhiteSpace(user.Email!))
        {
            await _emailSenderService.SendEmailAsync(
                user.Email,
                "MusicApp :: Confirm your email",
                html
            );
        }

        return new AuthResultDto
        {
            Succeeded = true,
            Message = "Successfull registration"
        };
    }

    private string BuildEmailConfirmationLink(Guid userId, string encodedToken)
    {
        string clientUrl = _configuration["ClientAppUrl"]
            ?? throw new ClientAppUrlNotConfiguredException();

        string confirmPath = _configuration["ConfirmEmailPath"] ?? "/confirm-email";

        return $"{clientUrl}{confirmPath}?userId={userId}&code={encodedToken}";
    }
}
