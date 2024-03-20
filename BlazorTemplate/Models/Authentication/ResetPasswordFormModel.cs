using Microsoft.Extensions.Options;

namespace BlazorTemplate.Models.User;

public class ResetPasswordFormModel
{
    public string NewPassword { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;
}


public class ResetPasswordFormModelValidator : AbstractValidator<ResetPasswordFormModel>
{
    public ResetPasswordFormModelValidator(IOptions<IdentityConfiguration> identityConfiguration)
    {
        IdentityConfiguration identitySettings = identityConfiguration.Value;
     
        RuleFor(p => p.NewPassword)
            .NotEmpty().WithMessage("New password cannot be empty")
            .MinimumLength(identitySettings.RequiredLength).WithMessage($"Your password length must be at least {identitySettings.RequiredLength} characters.")
            .MaximumLength(identitySettings.MaxLength).WithMessage($"Your password length must not exceed {identitySettings.MaxLength} characters.")
            .Matches(identitySettings.RequireUpperCase ? @"[A-Z]+" : string.Empty).WithMessage("Your password must contain at least one upper case character")
            .Matches(identitySettings.RequireLowerCase ? @"[a-z]+" : string.Empty).WithMessage("Your password must contain at least one lower case character")
            .Matches(identitySettings.RequireDigit ? @"[0-9]+" : string.Empty).WithMessage("Your password must contain at least one upper digit")
            .Matches(identitySettings.RequireNonAlphanumeric ? @"[\@\!\?\*\.]+" : string.Empty).WithMessage("Your password must contain only alphanumeric character");

        RuleFor(x => x.ConfirmPassword)
             .Equal(x => x.NewPassword).WithMessage("New Password mismatch");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        ValidationResult? result =
            await ValidateAsync(ValidationContext<ResetPasswordFormModel>.CreateWithOptions((ResetPasswordFormModel)model,
                x => x.IncludeProperties(propertyName)));
        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}