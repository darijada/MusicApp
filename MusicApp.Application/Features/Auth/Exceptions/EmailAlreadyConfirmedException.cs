namespace MusicApp.Application.Features.Auth.Exceptions;

public class EmailAlreadyConfirmedException : Exception
{
    public EmailAlreadyConfirmedException()
        : base("Email has already been confirmed.") { }
}

