using Microsoft.AspNetCore.Hosting;
using MusicApp.Application.Features.Email.Services;

namespace MusicApp.Infrastructure.Services.Email;

public class EmailTemplateService : IEmailTemplateService
{
    public string GetTemplate(string featureName, string templateName, Dictionary<string, string> tokens)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Features", featureName, "EmailTemplates", templateName);

        if (!File.Exists(path))
            throw new FileNotFoundException("Email template not found", path);

        var html = File.ReadAllText(path);

        foreach (var token in tokens)
            html = html.Replace(token.Key, token.Value);

        return html;
    }

}

