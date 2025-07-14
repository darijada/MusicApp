using MediatR;
using Microsoft.AspNetCore.Identity;
using MusicApp.Application.Features.Auth.Commands;
using MusicApp.Application.Features.Auth.Dtos;
using MusicApp.Application.Features.Auth.Exceptions;
using MusicApp.Application.Features.Auth.Services;
using MusicApp.Core.Entities.Auth;

namespace MusicApp.Application.Features.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResultDto>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshService;

        public LoginHandler(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IJwtService jwtService,
            IRefreshTokenService refreshService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _refreshService = refreshService;
        }

        public async Task<AuthResultDto> Handle(LoginCommand request, CancellationToken ct)
        {
            LoginDto dto = request.Dto;

            User? user = await _userManager.FindByNameAsync(dto.UserName) ?? throw new InvalidCredentialsException();

            if (!user.EmailConfirmed)
                throw new EmailNotConfirmedException();

            SignInResult signInRes = await _signInManager
                .CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);
            if (!signInRes.Succeeded)
                throw new InvalidCredentialsException();

            var (accessToken, refreshTokenEntity) = _jwtService.GenerateTokens(user);

            await _refreshService.CreateRefreshTokenAsync(refreshTokenEntity, ct);

            return new AuthResultDto
            {
                Succeeded = true,
                AccessToken = accessToken,
                RefreshToken = refreshTokenEntity.Token,
                Message = "Login successful."
            };
        }
    }
}
