namespace Blazor.Application.Users.Commands.UpdateUser;


public class UpdateUserCommand(int Id, string UserName, string Email, bool LockoutEnabled, List<int> RolesIds) : IRequest<Result>
{
    public int Id { get; set; } = Guard.Against.NegativeOrZero(Id);

    public string Name { get; set; } = Guard.Against.NullOrEmpty(UserName);

    public string Email { get; set; } = Guard.Against.NullOrEmpty(Email);

    public bool LockoutEnabled { get; set; } = LockoutEnabled;

    public List<int> RolesIds { get; set; } = RolesIds;
}
