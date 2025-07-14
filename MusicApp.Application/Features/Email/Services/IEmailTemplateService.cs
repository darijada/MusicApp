namespace MusicApp.Application.Features.Email.Services;

public interface IEmailTemplateService
{
    public string GetTemplate(string featureName, string templateName, Dictionary<string, string> tokens);

}
