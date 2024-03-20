namespace Blazor.Infrastructure.Mail;

public class MailService(IFluentEmail fluentEmail, ILogger<MailService> logger) : IMailService
{
    private const string KTemplatePath = "BlazorTemplate.Resources.EmailTemplates.{0}.cshtml";

    private readonly IFluentEmail _fluentEmail = fluentEmail;

    private readonly ILogger<MailService> _logger = logger;


    public Task<SendResponse> SendAsync(string to, string subject, string body)
    {
        Guard.Against.NullOrEmpty(to);

        try
        {
            return _fluentEmail
                .To(to)
                .Subject(subject)
                .Body(body, true)
                .SendAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error sending an email to {Unknown} with subject {Subject}", to, subject);
            throw;
        }
    }


    public Task<SendResponse> SendAsync(string to, string subject, string template, object model)
    {
        try
        {
            return _fluentEmail
                .To(to)
                .Subject(subject)
                .UsingTemplateFromEmbedded(string.Format(KTemplatePath, template), model, Assembly.GetEntryAssembly())
                .SendAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error sending an email to {Unknown} with subject {Subject} and template {Template}", to, subject, template);
            throw;
        }
    }
}
