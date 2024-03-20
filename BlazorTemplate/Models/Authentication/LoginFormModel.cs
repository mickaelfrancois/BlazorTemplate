namespace BlazorTemplate.Models.Authentication;

public class LoginFormModel
{
    public string? UserName { get; set; }

    public string? Password { get; set; }
}


public class LoginFormModelValidator : AbstractValidator<LoginFormModel>
{
    public LoginFormModelValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Your username cannot be empty.");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Your password cannot be empty");
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        ValidationResult? result =
            await ValidateAsync(ValidationContext<LoginFormModel>.CreateWithOptions((LoginFormModel)model,
                x => x.IncludeProperties(propertyName)));

        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}