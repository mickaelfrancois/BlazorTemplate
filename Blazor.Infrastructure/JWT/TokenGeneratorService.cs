namespace Blazor.Infrastructure.JWT;

public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    private readonly IConfirmEmailTokenGenerator _confirmEmailTokenGenerator;

    private readonly IForgotPasswordTokenGenerator _forgotPasswordTokenGenerator;


    public TokenGeneratorService(IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator, IConfirmEmailTokenGenerator confirmEmailTokenGenerator, IForgotPasswordTokenGenerator forgotPasswordTokenGenerator)
    {
        _accessTokenGenerator = Guard.Against.Null(accessTokenGenerator);
        _refreshTokenGenerator = Guard.Against.Null(refreshTokenGenerator);
        _confirmEmailTokenGenerator = Guard.Against.Null(confirmEmailTokenGenerator);
        _confirmEmailTokenGenerator = Guard.Against.Null(confirmEmailTokenGenerator);
        _forgotPasswordTokenGenerator = Guard.Against.Null(forgotPasswordTokenGenerator);
    }

    public string GenerateAccessToken(ClaimsPrincipal user)
    {
        return _accessTokenGenerator.GenerateToken(user);
    }

    public string GenerateRefreshToken(ClaimsPrincipal user)
    {
        return _refreshTokenGenerator.GenerateToken(user);
    }

    public string GenerateConfirmToken(ClaimsPrincipal user)
    {
        return _confirmEmailTokenGenerator.GenerateToken(user);
    }

    public string GenerateForgotPasswordToken(ClaimsPrincipal user)
    {
        return _forgotPasswordTokenGenerator.GenerateToken(user);
    }
}
