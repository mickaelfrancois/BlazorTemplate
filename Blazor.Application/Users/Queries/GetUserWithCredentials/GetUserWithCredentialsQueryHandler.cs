namespace Blazor.Application.Users.Queries.GetUserWithCredentials;

public class GetUserWithCredentialsQueryHandler(DataAccess dataAccess) : IRequestHandler<GetUserWithCredentialsQuery, UserDto?>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<UserDto?> Handle(GetUserWithCredentialsQuery request, CancellationToken cancellationToken)
    {
        string passwordHash = SecurityService.GetSha256Hash(request.Password);

        return await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                            .Where("name", request.Name)
                                            .Where("password", passwordHash)
                                            .FirstOrDefaultAsync<UserDto?>(cancellationToken: cancellationToken);
    }
}
