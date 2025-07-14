using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicApp.Core.Entities.Auth;
using MusicApp.Application.Features.Auth.Queries;

namespace MusicApp.Application.Features.Auth.Handlers;

public class SearchUsernamesHandler : IRequestHandler<SearchUsernamesQuery, List<string>>
{
    private readonly UserManager<User> _userManager;

    public SearchUsernamesHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<string>> Handle(SearchUsernamesQuery request, CancellationToken cancellationToken)
    {
        var usersQuery = _userManager.Users
            .AsNoTracking()
            .Where(u => u.IsActive);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var termLower = request.SearchTerm.Trim().ToLower();
            usersQuery = usersQuery
                .Where(u => u.UserName!.ToLower().Contains(termLower));
        }

        return await usersQuery
            .OrderBy(u => u.UserName)
            .Select(u => u.UserName!)
            .ToListAsync(cancellationToken);
    }
}
