namespace Blazor.Application.Users.Queries.GetUsers;

public class GetUsersQueryHandler(DataAccess dataAccess) : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _dataAccess.QueryFactory.Query(UserDto.EntityName).GetAsync<UserDto>(null, null, cancellationToken);
    }
}
