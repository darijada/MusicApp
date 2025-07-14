using MediatR;

namespace MusicApp.Application.Features.Auth.Queries;

/// <summary>
/// Query for searching active usernames
/// </summary>
public record SearchUsernamesQuery(string? SearchTerm) : IRequest<List<string>>;