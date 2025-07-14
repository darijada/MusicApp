using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MusicApp.Application.Features.Auth.Queries;
using MusicApp.Core.Entities.Auth;

namespace MusicApp.Application.Features.Auth.Handlers
{
    public class GetAllUsernamesQueryHandler : IRequestHandler<GetAllUsernamesQuery, List<string>>
    {
        private readonly UserManager<User> _userManager;

        public GetAllUsernamesQueryHandler(UserManager<User> userManager)
            => _userManager = userManager;

        public async Task<List<string>> Handle(GetAllUsernamesQuery request, CancellationToken ct)
        {
            return await _userManager.Users
                .AsNoTracking()
                .Where(u => u.IsActive)
                .OrderBy(u => u.UserName)
                .Select(u => u.UserName!)
                .ToListAsync(ct);
        }
    }
}
