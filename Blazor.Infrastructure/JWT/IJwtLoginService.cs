namespace Blazor.Infrastructure.JWT;

public interface IJwtLoginService
{
    AuthenticatedUserResponse GetToken(ClaimsPrincipal user);
}