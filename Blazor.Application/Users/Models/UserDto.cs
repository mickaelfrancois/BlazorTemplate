namespace Blazor.Application.Users.Models;

public class UserDto
{
    public const string EntityName = "users";

    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool LockoutEnabled { get; set; }

    public bool EmailConfirmed { get; set; }
}
