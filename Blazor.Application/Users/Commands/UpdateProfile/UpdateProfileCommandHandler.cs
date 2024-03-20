namespace Blazor.Application.Users.Commands.UpdateProfile;


public class UpdateProfileCommandHandler(DataAccess dataAccess) : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly DataAccess _dataAccess = Guard.Against.Null(dataAccess);


    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        if (await CheckUserExistsAsync(request, cancellationToken))
            return Result.Fail("Another user already exists!");

        await UpdateUserAsync(request, cancellationToken);

        return Result.Ok();
    }


    private async Task<bool> CheckUserExistsAsync(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<int> result = await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                .Where("name", request.Name)
                                .OrWhere("email", request.Email)
                                .Select("id")
                                .GetAsync<int>(null, null, cancellationToken);

        return result.Any(c => c != request.Id);
    }


    private async Task UpdateUserAsync(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        await _dataAccess.QueryFactory.Query(UserDto.EntityName)
                                      .Where("id", request.Id)
                                      .UpdateAsync(new
                                      {
                                          request.Name,
                                          request.Email
                                      }, null, null, cancellationToken);
    }
}