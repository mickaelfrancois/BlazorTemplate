using Microsoft.Extensions.Options;

namespace BlazorTemplate.Models.Authentication;

public class ForgotPasswordFormModel
{
    public string? Email { get; set; }
}


public class ForgotPasswordFormModelValidator : AbstractValidator<ForgotPasswordFormModel>
{
    public ForgotPasswordFormModelValidator(IOptions<IdentityConfiguration> identityConfiguration)
    {
        IdentityConfiguration identitySettings = identityConfiguration.Value;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Your email cannot be empty")
            .MaximumLength(255)
            .EmailAddress();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        ValidationResult? result =
            await ValidateAsync(ValidationContext<ForgotPasswordFormModel>.CreateWithOptions((ForgotPasswordFormModel)model,
                x => x.IncludeProperties(propertyName)));
        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}