namespace Blazor.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(int Id) : IRequest<Result<UserVm>>
{
    public int Id { get; } = Guard.Against.NegativeOrZero(Id);
}
