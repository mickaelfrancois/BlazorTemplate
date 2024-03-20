using Blazor.Application.Roles.Models;
using Blazor.Application.UserRoles.Models;

namespace Blazor.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(DataAccess dataAccess) : IRequestHandler<GetUserByIdQuery, Result<UserVm>>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);

    public async Task<Result<UserVm>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<RoleDto> roles = await _dataAccess.QueryFactory.Query(UserRolesDto.EntityName)
                        .Select("roles.name", "roles.id")
                        .Join(RoleDto.EntityName, "roles.id", "userroles.roleId")
                        .Where("userId", request.Id).GetAsync<RoleDto>(null, null, cancellationToken);

        UserVm? user = await _dataAccess.QueryFactory.Query(UserDto.EntityName).Where("id", request.Id).FirstOrDefaultAsync<UserVm?>(cancellationToken: cancellationToken);

        if (user == null)
            return Result.Fail("User not found!");

        user.Roles = roles.ToList();

        return Result.Ok(user);
    }
}
