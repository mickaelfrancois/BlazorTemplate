using System.Security.Cryptography;


namespace Blazor.Application.Security;

public static class SecurityService
{
    public static string GetSha256Hash(string input)
    {
        Guard.Against.NullOrEmpty(input);

        byte[] byteValue = Encoding.UTF8.GetBytes(input);
        byte[] byteHash = SHA256.HashData(byteValue);

        return Convert.ToBase64String(byteHash);
    }
}
