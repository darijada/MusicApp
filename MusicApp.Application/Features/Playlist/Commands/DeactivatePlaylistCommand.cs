using MediatR;

namespace MusicApp.Application.Features.Playlist.Commands
{
    public record DeactivatePlaylistCommand(Guid PlaylistId, Guid UserId) : IRequest;
}
