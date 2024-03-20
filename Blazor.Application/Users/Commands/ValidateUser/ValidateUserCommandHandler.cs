using SqlKata;

namespace Blazor.Application.Users.Commands.ValidateUser;

public class ValidateUserCommandHandler(DataAccess dataAccess) : IRequestHandler<ValidateUserCommand, Result>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<Result> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
    {
        await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                        .Where("id", request.Id)
                                        .UpdateAsync(new
                                        {
                                            EmailConfirmed = true,
                                            VerifyToken = Expressions.UnsafeLiteral("null"),
                                        }, null, null, cancellationToken);

        return Result.Ok();
    }
}
