using Blazor.Application.Roles.Models;

namespace Blazor.Application.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<IEnumerable<RoleDto>>
{
}
