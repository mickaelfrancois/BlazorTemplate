namespace Blazor.Application.Users.Commands.UpdatePassword;

public record UpdatePasswordCommand(int Id, string OldPassword, string NewPassword) : IRequest<Result>
{
    public int Id { get; set; } = Guard.Against.NegativeOrZero(Id);

    public string OldPassword { get; set; } = Guard.Against.NullOrEmpty(OldPassword);

    public string NewPassword { get; set; } = Guard.Against.NullOrEmpty(NewPassword);
}
