using Blazor.Application.Users.Commands.DeleteUser;

namespace BlazorApplication.UnitTests.Users;

[Collection("Sequential")]
public class DeleteUserTests()
{
    private readonly DatabaseFixture _fixture = new();


    [Fact(DisplayName = "Delete user with valid input data")]
    public async void DeleteUser_ValidInputData()
    {
        _fixture.CreateUserTable()
           .AddRow(1, new ColumnValueModel("name", "name1"));
        _fixture.CreateUserRolesTable()
            .AddRow(1, new ColumnValueModel("roleId", 1), new ColumnValueModel("userId", 1));

        DeleteUserCommandHandler sut = new(_fixture.DataAccess);
        DeleteUserCommand command = new(1);

        Result result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        int userRolesCount = Convert.ToInt32(_fixture.DataAccess.ExecuteScalar("SELECT COUNT(*) FROM userroles WHERE roleid = 1 AND userId = 1"));
        Assert.Equal(0, userRolesCount);

        int userCount = Convert.ToInt32(_fixture.DataAccess.ExecuteScalar("SELECT COUNT(*) FROM users WHERE id = 1"));
        Assert.Equal(0, userCount);
    }
}