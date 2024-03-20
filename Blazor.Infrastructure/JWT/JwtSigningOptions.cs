using Microsoft.IdentityModel.Tokens;

namespace Blazor.Infrastructure.JWT;

public class JwtSigningOptions
{
    public int ExpirationMinutes { get; set; } = 120;

    public string Algorithm { get; init; } = SecurityAlgorithms.HmacSha256;

    public string SigningKey { get; set; } = string.Empty;
}
