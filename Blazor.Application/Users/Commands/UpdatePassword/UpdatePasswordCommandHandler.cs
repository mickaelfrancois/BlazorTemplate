namespace Blazor.Application.Users.Commands.UpdatePassword;

public class UpdatePasswordCommandHandler(DataAccess dataAccess) : IRequestHandler<UpdatePasswordCommand, Result>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        string oldPassword = await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                .Where("id", request.Id)
                                .Select("password")
                                .FirstOrDefaultAsync<string>(null, null, cancellationToken);

        if (ComparePassword(oldPassword, request.OldPassword) == false)
            return Result.Fail("Current password is invalid!");

        await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                      .Where("id", request.Id)
                                      .UpdateAsync(new
                                      {
                                          Password = SecurityService.GetSha256Hash(request.NewPassword),
                                      }, null, null, cancellationToken);

        return Result.Ok();
    }


    private static bool ComparePassword(string currentPassword, string oldPassword)
    {
        string oldPasswordHash = SecurityService.GetSha256Hash(oldPassword);

        return currentPassword == oldPasswordHash;
    }
}
