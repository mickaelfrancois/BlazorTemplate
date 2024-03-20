using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blazor.Infrastructure.JWT;

public class JwtConfiguration
{
    public const string Key = "JWT";

    public string Issuer { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public JwtSigningOptions AccessSigningOptions { get; set; } = new();

    public JwtSigningOptions RefreshSigningOptions { get; set; } = new();

    public JwtSigningOptions ConfirmEmailOptions { get; set; } = new();

    public JwtSigningOptions ForgotPasswordOptions { get; set; } = new();



    public TokenValidationParameters GetAccessTokenValidationParameters()
    {
        byte[] signingKeyBytes = Encoding.UTF8.GetBytes(AccessSigningOptions.SigningKey);

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    }


    public TokenValidationParameters GetRefreshTokenValidationParameters()
    {
        byte[] signingKeyBytes = Encoding.UTF8.GetBytes(RefreshSigningOptions.SigningKey);

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    }


    public TokenValidationParameters GetConfirmEmailTokenValidationParameters()
    {
        byte[] signingKeyBytes = Encoding.UTF8.GetBytes(ConfirmEmailOptions.SigningKey);

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    }


    public TokenValidationParameters GetForgotPasswordTokenValidationParameters()
    {
        byte[] signingKeyBytes = Encoding.UTF8.GetBytes(ForgotPasswordOptions.SigningKey);

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    }
}

