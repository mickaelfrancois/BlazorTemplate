namespace Blazor.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(DataAccess dataAccess) : IRequestHandler<RegisterUserCommand, Result<int>>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<Result<int>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        request.Password = SecurityService.GetSha256Hash(request.Password);


        bool userAlreadyExists = await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                .Where("name", request.Name)
                                .OrWhere("email", request.Email)
                                .ExistsAsync(null, null, cancellationToken);

        if (userAlreadyExists)
            return Result.Fail("User already exists!");

        int id = await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                                .InsertAsync(request, null, null, cancellationToken);

        return Result.Ok(id);
    }
}
