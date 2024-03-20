using FluentResults;

namespace Blazor.Infrastructure.JWT;

public class TokenValidatorService(JwtSecurityTokenHandler tokenHandler, IOptions<JwtConfiguration> options)
{
    private readonly JwtSecurityTokenHandler _tokenHandler = Guard.Against.Null(tokenHandler);

    private readonly TokenValidationParameters _validationAccessParameters = options.Value.GetAccessTokenValidationParameters()!;

    private readonly TokenValidationParameters _validationRefreshParameters = options.Value.GetRefreshTokenValidationParameters()!;

    private readonly TokenValidationParameters _validationConfirmEmailParameters = options.Value.GetConfirmEmailTokenValidationParameters()!;

    private readonly TokenValidationParameters _validationForgotPasswordParameters = options.Value.GetForgotPasswordTokenValidationParameters()!;


    public async Task<Result<ClaimsIdentity>> ValidateAccessTokenAsync(string token)
    {
        Guard.Against.NullOrEmpty(token);


        TokenValidationResult result = await _tokenHandler.ValidateTokenAsync(token, _validationAccessParameters);

        if (result.IsValid)
            return Result.Ok(result.ClaimsIdentity);
        else
            return Result.Fail("Invalid access token!");
    }


    public async Task<Result<ClaimsIdentity>> ValidateRefreshTokenAsync(string token)
    {
        Guard.Against.NullOrEmpty(token);


        TokenValidationResult result = await _tokenHandler.ValidateTokenAsync(token, _validationRefreshParameters);

        if (result.IsValid)
            return Result.Ok(result.ClaimsIdentity);
        else
            return Result.Fail("Invalid refresh token!");
    }


    public async Task<Result<ClaimsIdentity>> ValidateConfirmEmailTokenAsync(string token)
    {
        Guard.Against.NullOrEmpty(token);


        TokenValidationResult result = await _tokenHandler.ValidateTokenAsync(token, _validationConfirmEmailParameters);

        if (result.IsValid)
            return Result.Ok(result.ClaimsIdentity);
        else
            return Result.Fail("Invalid confirm email token!");
    }


    public async Task<Result<ClaimsIdentity>> ValidateForgotPasswordTokenAsync(string token)
    {
        Guard.Against.NullOrEmpty(token);


        TokenValidationResult result = await _tokenHandler.ValidateTokenAsync(token, _validationForgotPasswordParameters);

        if (result.IsValid)
            return Result.Ok(result.ClaimsIdentity);
        else
            return Result.Fail("Invalid reset password token!");
    }
}
