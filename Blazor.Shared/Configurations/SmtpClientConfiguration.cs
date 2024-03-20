using MailKit.Security;

namespace Blazor.Shared.Configurations;

public class SmtpClientConfiguration
{
    public const string Key = "SmtpClient";

    public string DefaultSender { get; set; } = string.Empty;

    public string Server { get; set; } = string.Empty;

    public int Port { get; set; } = 25;

    public string User { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool UseSsl { get; set; } = false;

    public bool RequiresAuthentication { get; set; } = false;

    public string PreferredEncoding { get; set; } = string.Empty;

    public bool UsePickupDirectory { get; set; } = false;

    public string MailPickupDirectory { get; set; } = string.Empty;

    public SecureSocketOptions? SocketOptions { get; set; }
}
