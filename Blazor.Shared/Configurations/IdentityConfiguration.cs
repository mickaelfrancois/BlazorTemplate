namespace Blazor.Shared.Configurations;

public class IdentityConfiguration
{
    public const string Key = "Identity";

    public bool RequireDigit { get; set; } = true;

    public int RequiredLength { get; set; } = 6;

    public int MaxLength { get; set; } = 32;

    public bool RequireNonAlphanumeric { get; set; } = true;

    public bool RequireUpperCase { get; set; } = true;

    public bool RequireLowerCase { get; set; } = false;
}