using Blazor.Application.UserRoles.Models;

namespace Blazor.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(DataAccess dataAccess) : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _dataAccess.BeginTransaction();

            await DeleteUserAsync(request, cancellationToken);
            await DeleteUserRolesAsync(request, cancellationToken);

            _dataAccess.CommitTransaction();
        }
        catch
        {
            _dataAccess.RollbackTransaction();
            throw;
        }


        return Result.Ok();
    }


    private async Task DeleteUserRolesAsync(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _dataAccess.QueryFactory.Query(UserRolesDto.EntityName)
                                     .Where("userId", request.Id)
                                     .AsDelete()
                                     .DeleteAsync(null, null, cancellationToken);
    }


    private async Task DeleteUserAsync(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                     .Where("id", request.Id)
                                     .AsDelete()
                                     .DeleteAsync(null, null, cancellationToken);
    }
}
