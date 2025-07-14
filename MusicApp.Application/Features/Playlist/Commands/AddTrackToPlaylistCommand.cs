using MediatR;
using MusicApp.Application.Features.Playlist.Dtos;

namespace MusicApp.Application.Features.Playlist.Commands;

/// <summary>
/// Command to add track to a playlist
/// </summary>
public record AddTrackToPlaylistCommand(Guid PlaylistId, AddTrackToPlaylistDto Dto, Guid UserId) : IRequest<Guid>;