using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Extensions;
using MusicApp.Application.Features.Auth.Exceptions;
using MusicApp.Application.Features.Playlist.Commands;
using MusicApp.Application.Features.Playlist.Dtos;
using MusicApp.Application.Features.Playlist.Queries;

namespace MusicApp.API.Features.Playlist;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlaylistController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlaylistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-playlist")]
    [Authorize]
    public async Task<IActionResult> CreatePlaylist([FromBody] CreatePlaylistDto dto)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UnauthorizedActionException();

        var (playlistId, playlistName) = await _mediator.Send(new CreatePlaylistCommand(dto, userId));

        return Ok(new
        {
            PlaylistId = playlistId,
            PlaylistName = playlistName,
            Message = "Playlist created successfully."
        });
    }

    [HttpGet("{playlistId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid playlistId)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UnauthorizedActionException();
       
        PlaylistDto result = await _mediator.Send(new GetPlaylistByIdQuery(playlistId, userId));
        
        return Ok(result);
    }

    [HttpGet("get_my_playlists")]
    [Authorize]
    public async Task<IActionResult> GetMyPlaylists()
    {
        if (!User.TryGetUserId(out var userId))
            throw new UnauthorizedActionException();

        var playlists = await _mediator.Send(new GetUserPlaylistsQuery(userId));
        return Ok(playlists);
    }

    [HttpGet("all_user_playlists/{ownerUserId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetAllUserPlaylists(Guid ownerUserId)
    {
        var playlists = await _mediator.Send(new GetUserPlaylistsQuery(ownerUserId));
        return Ok(playlists);
    }

    [HttpPost("{playlistId:guid}/add-track")]
    [Authorize]
    public async Task<IActionResult> AddTrackToPlaylist(Guid playlistId, [FromBody] AddTrackToPlaylistDto dto)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UnauthorizedActionException();

        Guid trackId = await _mediator.Send(new AddTrackToPlaylistCommand(playlistId, dto, userId));

        return Ok(new
        {
            PlaylistId = playlistId,
            UserId = userId,
            Message = "Track added to playlist successfully."
        });
    }

    [HttpPost("{playlistId:guid}/remove-track")]
    [Authorize]
    public async Task<IActionResult> AddTrackToPlaylist(Guid playlistId, [FromBody] Guid trackId)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UnauthorizedActionException();

        await _mediator.Send(new RemoveTrackFromPlaylistCommand(playlistId, trackId, userId));

        return Ok(new
        {
            PlaylistId = playlistId,
            UserId = userId,
            Message = "Track removed from playlist successfully."
        });
    }

    [HttpPost("{playlistId:guid}/deactivate-playlist")]
    [Authorize]
    public async Task<IActionResult> DeactivatePlaylist(Guid playlistId)
    {
        if (!User.TryGetUserId(out var userId))
            throw new UnauthorizedActionException();

        await _mediator.Send(new DeactivatePlaylistCommand(playlistId, userId));

        return Ok(new
        {
            PlaylistId = playlistId,
            Message = "Playlist deactivated successfully."
        });
    }

    [HttpPost("{playlistId:guid}/share-playlist")]
    [Authorize]
    public async Task<IActionResult> SharePlaylist(Guid playlistId, [FromBody] PlaylistShareDto dto)
    {
        if (!User.TryGetUserId(out var ownerUserId))
            throw new UnauthorizedActionException();

        await _mediator.Send(new SharePlaylistCommand(playlistId, dto.UserId, ownerUserId));

        return Ok(new
        {
            PlaylistId = playlistId,
            OwnerUserId = ownerUserId,
            TargetUserId = dto.UserId,
            Message = "Playlist successfully shared."
        });
    }

    [HttpPost("{playlistId:guid}/unshare-playlist")]
    [Authorize]
    public async Task<IActionResult> UnsharePlaylist(Guid playlistId, [FromBody] PlaylistShareDto dto)
    {
        if (!User.TryGetUserId(out var ownerUserId))
            throw new UnauthorizedActionException();

        await _mediator.Send(new UnsharePlaylistCommand(playlistId, dto.UserId, ownerUserId));

        return Ok(new
        {
            PlaylistId = playlistId,
            OwnerUserId = ownerUserId,
            TargetUserId = dto.UserId,
            Message = "Playlist successfully unshared."
        });
    }
}
