namespace MusicApp.Application.Features.Auth.Exceptions;

/// <summary>
/// Thrown when the ClientAppUrl setting is missing or empty in configuration.
/// </summary>
public class ClientAppUrlNotConfiguredException : Exception
{
    public ClientAppUrlNotConfiguredException()
        : base("Configuration value 'ClientAppUrl' is not set. Please configure the client application URL under 'ClientAppUrl' in appsettings.")
    {
    }

    public ClientAppUrlNotConfiguredException(string message)
        : base(message)
    {
    }
}
