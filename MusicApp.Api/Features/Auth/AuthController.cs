using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Features.Auth.Commands;
using MusicApp.Application.Features.Auth.Dtos;
using MusicApp.Application.Features.Auth.Exceptions;
using MusicApp.Application.Extensions;


namespace MusicApp.Api.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
        => _mediator = mediator;

    /// <summary>
    /// Registers a new user
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _mediator.Send(new RegisterCommand(dto));
        return Ok(new { message = result.Message });
    }

    /// <summary>
    /// Confirms a user's email address
    /// </summary>
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(
        [FromQuery] Guid userId,
        [FromQuery] string code)
    {
        var result = await _mediator.Send(new ConfirmEmailCommand(userId, code));
        return Ok(new { message = result.Message });
    }

    /// <summary>
    /// Logs in a user and returns a JWT token
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var result = await _mediator.Send(new LoginCommand(dto));
        return Ok(result);
    }

    /// <summary>
    /// Logs out the user by revoking the provided refresh token
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (!User.TryGetUserId(out var userId))
            throw new UnauthorizedActionException();

        await _mediator.Send(new LogoutCommand(userId));
        return NoContent();
    }


    /// <summary>
    /// Refreshes an expired access token using a valid refresh token
    /// </summary>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(dto.RefreshToken));
        return Ok(result);
    }

    /// <summary>
    /// Deactivates (soft-deletes) the current user's account and sends a confirmation email
    /// </summary>
    [Authorize]
    [HttpPost("deactivate")]
    public async Task<IActionResult> Deactivate()
    {
        if (!User.TryGetUserId(out var userId))
            throw new UnauthorizedActionException();

        var result = await _mediator.Send(new DeactivateAccountCommand(userId));
        return Ok(result);
    }
}
