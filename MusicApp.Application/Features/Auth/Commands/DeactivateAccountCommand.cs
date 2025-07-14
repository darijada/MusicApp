using MediatR;
using MusicApp.Application.Features.Auth.Dtos;

namespace MusicApp.Application.Features.Auth.Commands;

/// <summary>
/// Command for deactivating user's account
/// </summary>
public record DeactivateAccountCommand(Guid UserId) : IRequest<AuthResultDto>;
