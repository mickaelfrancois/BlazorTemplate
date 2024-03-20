namespace Blazor.Infrastructure.JWT;

public interface IConfirmEmailTokenGenerator : ITokenGenerator { }


public class ConfirmEmailTokenGenerator(JwtSecurityTokenHandler tokenHandler, IOptions<JwtConfiguration> options) : IConfirmEmailTokenGenerator
{
    private readonly JwtSecurityTokenHandler _tokenHandler = Guard.Against.Null(tokenHandler);

    private readonly JwtConfiguration _options = Guard.Against.Null(options.Value);


    public string GenerateToken(ClaimsPrincipal user)
    {
        Guard.Against.Null(user);

        byte[] keyBytes = Encoding.UTF8.GetBytes(_options.ConfirmEmailOptions.SigningKey);
        SymmetricSecurityKey symmetricKey = new(keyBytes);

        SigningCredentials credentials = new(symmetricKey, SecurityAlgorithms.HmacSha256);
        JwtHeader header = new(credentials);

        JwtPayload payload = new(
            _options.Issuer!,
            _options.Audience!,
            user.Claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_options.ConfirmEmailOptions.ExpirationMinutes)
        );

        return _tokenHandler.WriteToken(new JwtSecurityToken(header, payload));
    }
}
