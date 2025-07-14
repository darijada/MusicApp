using MediatR;
using MusicApp.Application.Features.Auth.Dtos;

namespace MusicApp.Application.Features.Auth.Commands;

/// <summary>
/// Command for confirming user's email address
/// </summary>
public record ConfirmEmailCommand(Guid UserId, string Code) : IRequest<AuthResultDto>;
