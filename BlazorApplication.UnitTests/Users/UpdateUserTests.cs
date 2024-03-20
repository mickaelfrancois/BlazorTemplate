using Blazor.Application.Users.Commands.UpdateUser;

namespace BlazorApplication.UnitTests.Users;

[Collection("Sequential")]
public class UpdateUserTests()
{
    private readonly DatabaseFixture _fixture = new();


    [Fact(DisplayName = "Update user with valid input data")]
    public async void UpdateUser_ValidInputData()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"));
        _fixture.CreateUserRolesTable()
            .AddRow(1, new ColumnValueModel("roleId", 1), new ColumnValueModel("userId", 1));

        UpdateUserCommandHandler sut = new(_fixture.DataAccess);
        UpdateUserCommand command = new(1, "name", "email@mail.com", true, [1]);

        Result result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        var user = _fixture.DataContext.GetTable("users").GetRowById(1);
        Assert.Equal("name", user["name"]);
        Assert.Equal("email@mail.com", user["email"]);
        Assert.True(Convert.ToBoolean(user["lockoutEnabled"]));
    }


    [Fact(DisplayName = "Update user with new name wich already exists")]
    public async void UpdateUser_NameAleadyExists()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"))
            .AddRow(2, new ColumnValueModel("name", "name2"));

        UpdateUserCommandHandler sut = new(_fixture.DataAccess);
        UpdateUserCommand command = new(1, "name2", "email@mail.com", true, [1]);

        Result result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailed);
    }


    [Fact(DisplayName = "Update user with new email wich already exists")]
    public async void UpdateUser_EmailAleadyExists()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"), new ColumnValueModel("email", "email1@mail.com"))
            .AddRow(2, new ColumnValueModel("name", "name2"), new ColumnValueModel("email", "email2@mail.com"));

        UpdateUserCommandHandler sut = new(_fixture.DataAccess);
        UpdateUserCommand command = new(1, "name", "email2@mail.com", true, [1]);

        Result result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailed);
    }


    [Fact(DisplayName = "Update user with new roles")]
    public async void UpdateUser_WithNewRoles()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"));
        _fixture.CreateUserRolesTable()
            .AddRow(1, new ColumnValueModel("roleId", 1), new ColumnValueModel("userId", 1));

        UpdateUserCommandHandler sut = new(_fixture.DataAccess);
        UpdateUserCommand command = new(1, "name", "email@mail.com", true, [2]);

        Result result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        int newUserRolesCount = Convert.ToInt32(_fixture.DataAccess.ExecuteScalar("SELECT COUNT(*) FROM userroles WHERE roleid = 2 AND userId = 1"));
        Assert.Equal(1, newUserRolesCount);

        int oldUserRolesCount = Convert.ToInt32(_fixture.DataAccess.ExecuteScalar("SELECT COUNT(*) FROM userroles WHERE roleid = 1 AND userId = 1"));
        Assert.Equal(0, oldUserRolesCount);
    }
}