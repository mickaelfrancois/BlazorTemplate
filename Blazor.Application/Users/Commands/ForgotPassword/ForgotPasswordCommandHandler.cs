namespace Blazor.Application.Users.Commands.ResetPassword;

public class ForgotPasswordCommandHandler(DataAccess dataAccess, IMailService mailService, ITokenGeneratorService tokenGenerator) : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);

    private readonly IMailService _mailService = Guard.Against.Null(mailService);

    private readonly ITokenGeneratorService _tokenGenerator = Guard.Against.Null(tokenGenerator);


    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        UserDto user = await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                .Where("email", request.Email)
                                .FirstOrDefaultAsync<UserDto>(null, null, cancellationToken);

        if (user == null)
            return Result.Fail("No user found by email!");

        string token = GenerateToken(user.Email);
        Uri validationUrl = new(new Uri(request.ValidationUri), token);

        SendResponse? sendMailResult = await _mailService.SendAsync(user.Email, "Reset your password", "_recoverypassword",
                                                          new
                                                          {
                                                              AppName = "BlazorAuth",
                                                              user.Email,
                                                              ValidationUrl = validationUrl
                                                          });
        if (sendMailResult.Successful)
            return Result.Ok();
        else
            return Result.Fail(sendMailResult.ErrorMessages.First());
    }


    private string GenerateToken(string email)
    {
        Claim[] claims = [new(ClaimTypes.Email, email),];

        ClaimsPrincipal claimPrincipal = new(new ClaimsIdentity(claims, "CustomAuth"));
        return _tokenGenerator.GenerateForgotPasswordToken(claimPrincipal);
    }
}
