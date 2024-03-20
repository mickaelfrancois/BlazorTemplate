namespace Blazor.Application.Users.Commands.UpdateProfile;

public class UpdateProfileCommand(int Id, string UserName, string Email) : IRequest<Result>
{
    public int Id { get; set; } = Guard.Against.NegativeOrZero(Id);

    public string Name { get; set; } = Guard.Against.NullOrEmpty(UserName);

    public string Email { get; set; } = Guard.Against.NullOrEmpty(Email);
}