namespace Blazor.Application.Users.Queries.GetUserByEmail;


public record GetUserByEmailQuery(string Email) : IRequest<Result<UserDto>>
{
    public string Email { get; } = Guard.Against.Null(Email);
}
