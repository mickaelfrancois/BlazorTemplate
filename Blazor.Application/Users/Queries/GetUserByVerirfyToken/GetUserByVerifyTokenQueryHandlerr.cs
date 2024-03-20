namespace Blazor.Application.Users.Queries.GetUserByVerirfyToken;

public class GetUserByVerifyTokenQueryHandler(DataAccess dataAccess) : IRequestHandler<GetUserByVerifyTokenQuery, Result<UserDto>>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<Result<UserDto>> Handle(GetUserByVerifyTokenQuery request, CancellationToken cancellationToken)
    {
        UserDto? user = await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                                      .Where("verifyToken", request.Token)
                                                      .FirstOrDefaultAsync<UserDto>(null, null, cancellationToken);

        if (user == null)
            return Result.Fail("User was not found!");
        else
            return Result.Ok(user);
    }
}
