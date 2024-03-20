namespace Blazor.Application.Users.Queries.GetUsers;

public record GetUsersQuery : IRequest<IEnumerable<UserDto>>
{
}
