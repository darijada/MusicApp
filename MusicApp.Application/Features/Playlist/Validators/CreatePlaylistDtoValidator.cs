using FluentValidation;
using MusicApp.Application.Features.Playlist.Dtos;

namespace MusicApp.Application.Features.Playlist.Validators;

public class CreatePlaylistDtoValidator : AbstractValidator<CreatePlaylistDto>
{
    public CreatePlaylistDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotNull().WithMessage("Name is required.")
            .NotEmpty().WithMessage("Name cannot be empty or whitespace.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");
    }
}
