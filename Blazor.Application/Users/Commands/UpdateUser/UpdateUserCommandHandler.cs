using Blazor.Application.UserRoles.Models;

namespace Blazor.Application.Users.Commands.UpdateUser;


public class UpdateUserCommandHandler(DataAccess dataAccess) : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {       
        if(await CheckUserExistsAsync(request, cancellationToken))
            return Result.Fail("Another user already exists!");
        
        List<int> currentUserRoles = await GetCurrentUserRolesAsync(request.Id);

        try
        {
            _dataAccess.BeginTransaction();

            await UpdateUserAsync(request, cancellationToken);
            await UpdateRolesAsync(request, currentUserRoles, cancellationToken);
            await DeleteRolesAsync(request, currentUserRoles, cancellationToken);

            _dataAccess.CommitTransaction();
        }
        catch
        {
            _dataAccess.RollbackTransaction();
            throw;
        }
        
        return Result.Ok();
    }


    private async Task<bool> CheckUserExistsAsync(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<int> result = await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                .Where("name", request.Name)
                                .OrWhere("email", request.Email)
                                .Select("id")
                                .GetAsync<int>(null, null, cancellationToken);

        return result.Any(c => c != request.Id);
    }


    private async Task UpdateUserAsync(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                      .Where("id", request.Id)        
                                      .UpdateAsync(new
                                      {
                                          request.Name,
                                          request.Email,
                                          request.LockoutEnabled
                                      }, null, null, cancellationToken);
    }


    private async Task UpdateRolesAsync(UpdateUserCommand request, List<int> currentUserRoles, CancellationToken cancellationToken)
    {
        List<int> rolesToInsert = [.. request.RolesIds];
        rolesToInsert.RemoveAll(c => currentUserRoles.Contains(c));

        foreach (int roleId in rolesToInsert)
        {
            await _dataAccess.QueryFactory.Query(UserRolesDto.EntityName)
                                    .InsertAsync(new UserRolesDto { RoleId = roleId, UserId = request.Id }, null, null, cancellationToken);
        }
    }


    private async Task DeleteRolesAsync(UpdateUserCommand request, List<int> currentUserRoles, CancellationToken cancellationToken)
    {
        List<int> rolesToDelete = [.. currentUserRoles];
        rolesToDelete.RemoveAll(c => request.RolesIds.Contains(c));

        foreach (int roleId in rolesToDelete)
        {
            await _dataAccess.QueryFactory.Query(UserRolesDto.EntityName)
                                    .Where("roleId", roleId)
                                    .Where("userId", request.Id)
                                    .AsDelete()
                                    .DeleteAsync(null, null, cancellationToken);
        }
    }


    private async Task<List<int>> GetCurrentUserRolesAsync(int userId)
    {
        IEnumerable<int> result = await _dataAccess.QueryFactory.Query(UserRolesDto.EntityName)
                                      .Where("userId", userId)
                                      .Select("roleid").GetAsync<int>();

        return result.ToList();
    }
}
