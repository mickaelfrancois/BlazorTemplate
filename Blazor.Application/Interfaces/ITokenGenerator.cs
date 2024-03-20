using System.Security.Claims;

namespace Blazor.Infrastructure.Mail;

public interface ITokenGenerator
{
    string GenerateToken(ClaimsPrincipal user);
}