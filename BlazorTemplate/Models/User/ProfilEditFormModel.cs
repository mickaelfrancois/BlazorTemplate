namespace BlazorTemplate.Models.User;

public class ProfilEditFormModel
{
    public string? UserName { get; set; }

    public string? Email { get; set; }
}


public class ProfilEditFormModelValidator : AbstractValidator<ProfilEditFormModel>
{
    public ProfilEditFormModelValidator(IConfiguration configuration)
    {
        var identitySettings = configuration.GetRequiredSection(IdentityConfiguration.Key).Get<IdentityConfiguration>();

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Your username cannot be empty.")
            .Length(2, 100).WithMessage("Username must be between 2 and 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Your email cannot be empty")
            .MaximumLength(255)
            .EmailAddress();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        ValidationResult? result =
            await ValidateAsync(ValidationContext<ProfilEditFormModel>.CreateWithOptions((ProfilEditFormModel)model,
                x => x.IncludeProperties(propertyName)));

        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}