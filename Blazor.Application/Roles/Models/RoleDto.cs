namespace Blazor.Application.Roles.Models;

public class RoleDto
{
    public const string EntityName = "roles";

    public int RoleId { get; set; }

    public string Name { get; set; } = string.Empty;
}
