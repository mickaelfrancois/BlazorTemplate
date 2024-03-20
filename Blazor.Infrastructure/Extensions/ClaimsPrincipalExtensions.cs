using Blazor.Application.Models;
using System.Security.Claims;

namespace Blazor.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static UserProfile GetUserProfileFromClaim(this ClaimsPrincipal claimsPrincipal)
    {
        UserProfile profile = new();

        if (claimsPrincipal.Identity?.IsAuthenticated ?? false)
        {
            profile.Id = claimsPrincipal.GetUserId();
            profile.Name = claimsPrincipal.GetUserName() ?? "";
            profile.Email = claimsPrincipal.GetEmail() ?? "";
        }

        return profile;
    }

    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimTypes.Email);
    }

    public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        return Convert.ToInt32(claimsPrincipal.FindFirstValue(ClaimTypes.Sid));
    }

    public static string? GetUserName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimTypes.Name);
    }

    public static string[] GetRoles(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
    }
}
