using Blazor.Application.Roles.Models;

namespace Blazor.Application.Users.Models;

public class UserVm : UserDto
{
    public List<RoleDto> Roles { get; set; } = [];
}
