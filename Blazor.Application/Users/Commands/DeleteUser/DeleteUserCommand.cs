namespace Blazor.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(int Id) : IRequest<Result>
{
    public int Id { get; set; } = Guard.Against.NegativeOrZero(Id);
}
