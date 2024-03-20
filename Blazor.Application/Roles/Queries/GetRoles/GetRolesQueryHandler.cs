using Blazor.Application.Roles.Models;

namespace Blazor.Application.Roles.Queries.GetRoles;

public class GetRolesQueryHandler(DataAccess dataAccess) : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _dataAccess.QueryFactory.Query(RoleDto.EntityName).GetAsync<RoleDto>(null, null, cancellationToken);
    }
}
