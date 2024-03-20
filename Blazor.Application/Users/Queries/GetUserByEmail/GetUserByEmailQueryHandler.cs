namespace Blazor.Application.Users.Queries.GetUserByEmail;

public class GetUserByEmailQueryHandler(DataAccess dataAccess) : IRequestHandler<GetUserByEmailQuery, Result<UserDto>>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);

    public async Task<Result<UserDto>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        UserDto? user = await _dataAccess.QueryFactory.Query(UserDto.EntityName).Where("email", request.Email).FirstOrDefaultAsync<UserDto?>(cancellationToken: cancellationToken);

        if (user == null)
            return Result.Fail("User not found!");

        return Result.Ok(user);
    }
}
