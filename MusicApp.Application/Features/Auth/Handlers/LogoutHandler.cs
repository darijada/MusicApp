using MediatR;
using MusicApp.Application.Features.Auth.Commands;
using MusicApp.Application.Features.Auth.Services;

public class LogoutHandler : IRequestHandler<LogoutCommand>
{
    private readonly IRefreshTokenService _refreshService;

    public LogoutHandler(IRefreshTokenService refreshService)
    {
        _refreshService = refreshService;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken ct)
    {
        await _refreshService.RevokeAllTokensForUserAsync(request.UserId, ct);
        return Unit.Value;
    }
}
