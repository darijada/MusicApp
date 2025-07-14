using FluentValidation;
using MusicApp.Application.Features.Playlist.Dtos;

namespace MusicApp.Application.Features.Playlist.Validators;

public class TrackDtoValidator : AbstractValidator<TrackDto>
{
    public TrackDtoValidator()
    {
        RuleFor(x => x.SpotifyTrackId)
            .NotEmpty()
            .WithMessage("SpotifyTrackId is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Track name is required and must be under 100 characters.");

        RuleFor(x => x.Artists)
            .NotEmpty()
            .WithMessage("At least one artist is required.");

        RuleForEach(x => x.Artists)
            .NotEmpty()
            .WithMessage("Artist name cannot be empty.");

        RuleFor(x => x.AlbumName)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Album name is required and must be under 200 characters.");
    }
}
