namespace Blazor.Infrastructure.Mail;

public interface ITokenGeneratorService
{
    string GenerateAccessToken(ClaimsPrincipal user);

    string GenerateConfirmToken(ClaimsPrincipal user);

    string GenerateForgotPasswordToken(ClaimsPrincipal user);

    string GenerateRefreshToken(ClaimsPrincipal user);
}