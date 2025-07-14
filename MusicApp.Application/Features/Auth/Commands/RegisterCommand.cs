using MediatR;
using MusicApp.Application.Features.Auth.Dtos;

namespace MusicApp.Application.Features.Auth.Commands;

/// <summary>
/// Command for registering a new user
/// </summary>
public record RegisterCommand(RegisterDto Dto) : IRequest<AuthResultDto>;
