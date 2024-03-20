namespace Blazor.Infrastructure.JWT;

public class JwtLoginService(ITokenGeneratorService tokenGenerator) : IJwtLoginService
{
    private readonly ITokenGeneratorService _tokenGenerator = Guard.Against.Null(tokenGenerator);


    public AuthenticatedUserResponse GetToken(ClaimsPrincipal user)
    {
        var accessToken = _tokenGenerator.GenerateAccessToken(user);
        var refreshToken = _tokenGenerator.GenerateRefreshToken(user);

        return new AuthenticatedUserResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
