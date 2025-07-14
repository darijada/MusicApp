using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Features.Auth.Queries;

namespace MusicApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //[Authorize]
    [HttpGet("search-usernames")]
    public async Task<IActionResult> Search([FromQuery] string? username)
    {
        var usernames = await _mediator.Send(new SearchUsernamesQuery(username));
        return Ok(usernames);
    }

    //[Authorize]
    [HttpGet("all-usernames")]
    public async Task<IActionResult> GetAll()
    {
        var usernames = await _mediator.Send(new GetAllUsernamesQuery());
        return Ok(usernames);
    }
}
