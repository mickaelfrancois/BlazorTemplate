using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blazor.Infrastructure.JWT;

public interface IRefreshTokenGenerator : ITokenGenerator { }


public class RefreshTokenGenerator(JwtSecurityTokenHandler tokenHandler, IOptions<JwtConfiguration> options) : IRefreshTokenGenerator
{
    private readonly JwtSecurityTokenHandler _tokenHandler = Guard.Against.Null(tokenHandler);

    private readonly JwtConfiguration _options = Guard.Against.Null(options.Value);


    public string GenerateToken(ClaimsPrincipal user)
    {
        Guard.Against.Null(user);

        byte[] keyBytes = Encoding.UTF8.GetBytes(_options.RefreshSigningOptions.SigningKey);
        SymmetricSecurityKey symmetricKey = new(keyBytes);

        SigningCredentials credentials = new(symmetricKey, SecurityAlgorithms.HmacSha256);
        JwtHeader header = new(credentials);

        JwtPayload payload = new(
            _options.Issuer!,
            _options.Audience!,
            user.Claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_options.RefreshSigningOptions.ExpirationMinutes)
        );

        return _tokenHandler.WriteToken(new JwtSecurityToken(header, payload));
    }
}
