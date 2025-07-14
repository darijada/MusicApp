using MediatR;
using MusicApp.Application.Features.Auth.Dtos;

namespace MusicApp.Application.Features.Auth.Commands;

/// <summary>
/// Command for user login
/// </summary>
public record LoginCommand(LoginDto Dto) : IRequest<AuthResultDto>;
