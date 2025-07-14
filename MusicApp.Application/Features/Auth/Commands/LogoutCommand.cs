using MediatR;
using MusicApp.Application.Features.Auth.Dtos;

namespace MusicApp.Application.Features.Auth.Commands;

/// <summary>
/// Command for user logout
/// </summary>
public record LogoutCommand(Guid UserId) : IRequest;
