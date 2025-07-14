using MediatR;
using MusicApp.Application.Features.Auth.Dtos;

namespace MusicApp.Application.Features.Auth.Commands;

/// <summary>
/// A command to request a new JWT access token using an existing refresh token
/// </summary>
public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResultDto>;
