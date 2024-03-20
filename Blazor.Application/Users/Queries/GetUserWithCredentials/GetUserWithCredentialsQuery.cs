namespace Blazor.Application.Users.Queries.GetUserWithCredentials;

public record class GetUserWithCredentialsQuery(string Username, string Password) : IRequest<UserDto>
{
    public string Name { get; } = Guard.Against.NullOrEmpty(Username);

    public string Password { get; } = Guard.Against.NullOrEmpty(Password);
}
