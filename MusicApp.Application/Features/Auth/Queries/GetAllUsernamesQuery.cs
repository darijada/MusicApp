using MediatR;

namespace MusicApp.Application.Features.Auth.Queries
{
    /// <summary>
    /// Query for retrieving all active usernames
    /// </summary>
    public record GetAllUsernamesQuery() : IRequest<List<string>>;
}
