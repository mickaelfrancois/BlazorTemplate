namespace Blazor.Application.Users.Commands.RegisterUser;

public class RegisterUserCommand(string UserName, string Password, string Email, string VerifyToken) : IRequest<Result<int>>
{
    public string Name { get; set; } = Guard.Against.NullOrEmpty(UserName);

    public string Email { get; set; } = Guard.Against.NullOrEmpty(Email);

    public string Password { get; set; } = Guard.Against.NullOrEmpty(Password);

    public string VerifyToken { get; set; } = Guard.Against.NullOrEmpty(VerifyToken);

    public bool EmailConfirmed { get; set; } = false;
}
