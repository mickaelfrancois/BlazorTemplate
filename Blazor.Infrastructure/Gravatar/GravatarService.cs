using System.Security.Cryptography;
using System.Text;

namespace Blazor.Infrastructure.Gravatar;

public class GravatarService : IGravatarService
{
    public string GetUserUrlProfile(string email, int size = 32)
    {
        Guard.Against.NullOrEmpty(email);

        string hashEmail = HashEmail(email.ToLower());

        return $"https://www.gravatar.com/avatar/{hashEmail}?s={size}&d=identicon";
    }


    private static string HashEmail(string email)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(email);
        byte[] hashBytes = MD5.HashData(inputBytes);

        return Convert.ToHexString(hashBytes).ToLower();
    }
}