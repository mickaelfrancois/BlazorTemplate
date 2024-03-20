namespace Blazor.Application.Users.Commands.ResetPassword;

public record ForgotPasswordCommand(string Email, string ValidationUri) : IRequest<Result>
{
    public string Email { get; set; } = Guard.Against.NullOrEmpty(Email);

    public string ValidationUri { get; set; } = Guard.Against.NullOrEmpty(ValidationUri);
}