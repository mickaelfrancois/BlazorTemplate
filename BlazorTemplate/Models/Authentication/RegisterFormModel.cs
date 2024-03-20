using Microsoft.Extensions.Options;

namespace BlazorTemplate.Models.Authentication;

public class RegisterFormModel
{
    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }
}


public class RegisterFormModelValidator : AbstractValidator<RegisterFormModel>
{
    public RegisterFormModelValidator(IOptions<IdentityConfiguration> identityConfiguration)
    {
        IdentityConfiguration identitySettings = identityConfiguration.Value;

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Your username cannot be empty.")
            .Length(2, 100).WithMessage("Username must be between 2 and 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Your email cannot be empty")
            .MaximumLength(255)
            .EmailAddress();

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Your password cannot be empty")
            .MinimumLength(identitySettings.RequiredLength).WithMessage($"Your password length must be at least {identitySettings.RequiredLength} characters.")
            .MaximumLength(identitySettings.MaxLength).WithMessage($"Your password length must not exceed {identitySettings.MaxLength} characters.")
            .Matches(identitySettings.RequireUpperCase ? @"[A-Z]+" : string.Empty).WithMessage("Your password must contain at least one upper case character")
            .Matches(identitySettings.RequireLowerCase ? @"[a-z]+" : string.Empty).WithMessage("Your password must contain at least one lower case character")
            .Matches(identitySettings.RequireDigit ? @"[0-9]+" : string.Empty).WithMessage("Your password must contain at least one upper digit")
            .Matches(identitySettings.RequireNonAlphanumeric ? @"[\@\!\?\*\.]+" : string.Empty).WithMessage("Your password must contain only alphanumeric character");

        RuleFor(x => x.ConfirmPassword)
             .Equal(x => x.Password).WithMessage("NewPassword mismatch");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        ValidationResult? result =
            await ValidateAsync(ValidationContext<RegisterFormModel>.CreateWithOptions((RegisterFormModel)model,
                x => x.IncludeProperties(propertyName)));
        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}