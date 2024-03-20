namespace Blazor.Application.Users.Commands.UpdatePassword;

public class ResetPasswordCommandHandler(DataAccess dataAccess) : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {      
        await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                      .Where("id", request.Id)
                                      .UpdateAsync(new
                                      {
                                          Password = SecurityService.GetSha256Hash(request.NewPassword),
                                      }, null, null, cancellationToken);

        return Result.Ok();
    }
}
