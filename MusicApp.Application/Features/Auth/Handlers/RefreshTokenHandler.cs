using MediatR;
using Microsoft.AspNetCore.Identity;
using MusicApp.Application.Features.Auth.Commands;
using MusicApp.Application.Features.Auth.Dtos;
using MusicApp.Application.Features.Auth.Exceptions;
using MusicApp.Application.Features.Auth.Services;
using MusicApp.Core.Entities.Auth;

public class RefreshTokenHandler
    : IRequestHandler<RefreshTokenCommand, AuthResultDto>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtService _jwtService;
    private readonly UserManager<User> _userManager;

    public RefreshTokenHandler(
        IRefreshTokenService refreshTokenService,
        IJwtService jwtService,
        UserManager<User> userManager)
    {
        _refreshTokenService = refreshTokenService;
        _jwtService = jwtService;
        _userManager = userManager;
    }

    public async Task<AuthResultDto> Handle(RefreshTokenCommand req, CancellationToken ct)
    {
        RefreshToken validRefreshToken = await _refreshTokenService.GetValidRefreshTokenAsync(req.RefreshToken, ct) ?? throw new InvalidRefreshTokenException();

        User? user = await _userManager.FindByIdAsync(validRefreshToken.UserId.ToString()) ?? throw new UserNotFoundException(validRefreshToken.UserId);

        if (!user.IsActive || !user.EmailConfirmed)
            throw new InvalidRefreshTokenException();

        await _refreshTokenService.RevokeTokenAsync(validRefreshToken.Token, ct);

        var (newAccess, newRefresh) = _jwtService.GenerateTokens(user);

        await _refreshTokenService.CreateRefreshTokenAsync(newRefresh, ct);

        return new AuthResultDto
        {
            Succeeded = true,
            AccessToken = newAccess,
            RefreshToken = newRefresh.Token,
            Message = "Tokens refreshed successfully"
        };
    }

}
