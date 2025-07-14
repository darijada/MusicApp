using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Features.Auth.Exceptions;
using MusicApp.Application.Features.Email.Services;
using MusicApp.Application.Features.Playlist.Commands;
using MusicApp.Application.Features.Playlist.Repositories;
using MusicApp.Core.Entities.Auth;
using PlaylistEntity = MusicApp.Core.Entities.Playlist.Playlist;

namespace MusicApp.Application.Features.Playlist.Handlers;

public class DeactivatePlaylistCommandHandler : IRequestHandler<DeactivatePlaylistCommand, Unit>
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IPlaylistShareRepository _playlistShareRepository;
    private readonly UserManager<User> _userManager;
    private readonly IEmailSenderService _emailSender;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IImageEmbedService _imageEmbedService;

    public DeactivatePlaylistCommandHandler(
        IPlaylistRepository playlistRepository,
        IPlaylistShareRepository playlistShareRepository,
        UserManager<User> userManager,
        IEmailSenderService emailSender,
        IEmailTemplateService emailTemplateService,
        IImageEmbedService imageEmbedService)
    {
        _playlistRepository = playlistRepository;
        _playlistShareRepository = playlistShareRepository;
        _userManager = userManager;
        _emailSender = emailSender;
        _emailTemplateService = emailTemplateService;
        _imageEmbedService = imageEmbedService;
    }

    public async Task<Unit> Handle(DeactivatePlaylistCommand request, CancellationToken cancellationToken)
    {
        await _playlistRepository.DeactivateAsync(request.PlaylistId, request.UserId, cancellationToken);

        var sharedUserIds = await _playlistShareRepository
            .GetUserIdsSharedWithAsync(request.PlaylistId, cancellationToken);

        if (!sharedUserIds.Any())
            return Unit.Value;

        User? ownerUser = await _userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        PlaylistEntity? playlist = await _playlistRepository.SearchByIdAsync(request.PlaylistId, request.UserId, cancellationToken);
        
        var logoBase64 = _imageEmbedService.GetImageAsBase64("Resources/Images/logo.png");

        foreach (var targetUserId in sharedUserIds)
        {
            var targetUser = await _userManager.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == targetUserId, cancellationToken);

            if (targetUser is null)
                continue;

            await _playlistShareRepository.DeactivateShareAsync(
                request.PlaylistId,
                request.UserId,
                targetUserId,
                cancellationToken);

            string html = _emailTemplateService.GetTemplate("Playlist", "PlaylistUnshareEmail.html", new()
            {
                { "{{LOGO_IMAGE}}", logoBase64 },
                { "{{CURRENT_YEAR}}", DateTime.UtcNow.Year.ToString() },
                { "{{OWNER_USER}}", ownerUser.UserName! },
                { "{{TARGET_USER}}", targetUser.UserName! },
                { "{{PLAYLIST_NAME}}", playlist.Name }
            });

            await _emailSender.SendEmailAsync(
                targetUser.Email!,
                "MusicApp :: Playlist is no longer shared with you",
                html);
        }

        return Unit.Value;
    }
}
