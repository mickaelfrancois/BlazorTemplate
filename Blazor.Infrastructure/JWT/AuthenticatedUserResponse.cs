namespace Blazor.Infrastructure.JWT;

public class AuthenticatedUserResponse
{
    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }
}
