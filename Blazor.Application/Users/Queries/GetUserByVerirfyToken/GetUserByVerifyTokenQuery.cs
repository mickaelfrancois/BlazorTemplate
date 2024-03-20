namespace Blazor.Application.Users.Queries.GetUserByVerirfyToken;

public record GetUserByVerifyTokenQuery(string Token) : IRequest<Result<UserDto>>
{
    public string Token { get; set; } = Guard.Against.NullOrEmpty(Token);
}
