using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Features.Auth.Exceptions;
using MusicApp.Application.Features.Email.Services;
using MusicApp.Application.Features.Playlist.Commands;
using MusicApp.Application.Features.Playlist.Repositories;
using MusicApp.Core.Entities.Auth;

namespace MusicApp.Application.Features.Playlist.Handlers;

public class SharePlaylistCommandHandler : IRequestHandler<SharePlaylistCommand, Unit>
{
    private readonly IPlaylistShareRepository _playlistShareRepository;
    private readonly IPlaylistRepository _playlistRepository;
    private readonly UserManager<User> _userManager;
    private readonly IEmailSenderService _emailSender;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IImageEmbedService _imageEmbedService;

    public SharePlaylistCommandHandler(
        IPlaylistShareRepository playlistShareRepository,
        IPlaylistRepository playlistRepository,
        UserManager<User> userManager,
        IEmailSenderService emailSender,
        IEmailTemplateService emailTemplateService,
        IImageEmbedService imageEmbedService)
    {
        _playlistShareRepository = playlistShareRepository;
        _playlistRepository = playlistRepository;
        _userManager = userManager;
        _emailSender = emailSender;
        _emailTemplateService = emailTemplateService;
        _imageEmbedService = imageEmbedService;

    }

    public async Task<Unit> Handle(SharePlaylistCommand request, CancellationToken cancellationToken)
    {
        User? ownerUser = await _userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == request.OwnerUserId);

        if (ownerUser is null)
            throw new UserNotFoundException(request.OwnerUserId);
        
        User? targetUser = await _userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == request.TargetUserId);

        if (targetUser is null)
            throw new UserNotFoundException(request.TargetUserId);

        string playlistName = await _playlistShareRepository.CreateShareAsync(
            request.PlaylistId,
            request.OwnerUserId,
            request.TargetUserId,
            cancellationToken
        );

        string logoBase64 = _imageEmbedService.GetImageAsBase64("Resources/Images/logo.png");

        string html = _emailTemplateService.GetTemplate("Playlist", "PlaylistShareEmail.html", new()
        {
            { "{{LOGO_IMAGE}}", logoBase64 },
            { "{{CURRENT_YEAR}}", DateTime.UtcNow.Year.ToString() },
            { "{{OWNER_USER}}", ownerUser.UserName! },
            { "{{TARGET_USER}}", targetUser.UserName! },
            { "{{PLAYLIST_NAME}}", playlistName }
        });

        await _emailSender.SendEmailAsync(
            targetUser.Email!,
            "MusicApp :: Playlist is shared with you",
            html
        );

        return Unit.Value;
    }
}
