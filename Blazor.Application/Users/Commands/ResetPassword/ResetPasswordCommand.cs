namespace Blazor.Application.Users.Commands.UpdatePassword;

public record ResetPasswordCommand(int Id, string NewPassword) : IRequest<Result>
{
    public int Id { get; set; } = Guard.Against.NegativeOrZero(Id);

    public string NewPassword { get; set; } = Guard.Against.NullOrEmpty(NewPassword);
}
