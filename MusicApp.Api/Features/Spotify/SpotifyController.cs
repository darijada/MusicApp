using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicApp.Application.Features.Spotify.Commands;

namespace MusicApp.Api.Features.Spotify;

[ApiController]
[Route("api/[controller]")]
public class SpotifyController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpotifyController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet("search-tracks")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Query is required.");

        var result = await _mediator.Send(new SearchTracksQuery(q));
        return Ok(result);
    }

    [HttpGet("search-track/{id}")]
    public async Task<IActionResult> GetTrackById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Track ID is required.");

        var result = await _mediator.Send(new GetTrackByIdQuery(id));
        return Ok(result);
    }
}
