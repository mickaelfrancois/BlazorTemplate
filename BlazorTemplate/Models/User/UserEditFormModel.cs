namespace BlazorTemplate.Models.User;

public class UserEditFormModel
{
    public string UserName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool LockoutEnabled { get; set; }

    public List<int> RolesIds { get; set; } = [];
}

public class UserEditFormModelValidator : AbstractValidator<UserEditFormModel>
{
    public UserEditFormModelValidator(IConfiguration configuration)
    {
        var identitySettings = configuration.GetRequiredSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username cannot be empty.")
            .Length(2, 100).WithMessage("Username must be between 2 and 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .MaximumLength(255)
            .EmailAddress();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        ValidationResult? result =
            await ValidateAsync(ValidationContext<UserEditFormModel>.CreateWithOptions((UserEditFormModel)model,
                x => x.IncludeProperties(propertyName)));
        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}