namespace Blazor.Application.Users.Commands.ValidateUser;

public record ValidateUserCommand(int Id) : IRequest<Result>
{
    public int Id { get; set; } = Guard.Against.NegativeOrZero(Id);
}
