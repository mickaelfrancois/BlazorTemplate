using Blazor.Application.Users.Commands.RegisterUser;

namespace BlazorApplication.UnitTests.Users;

[Collection("Sequential")]
public class RegisterUserTests()
{
    private readonly DatabaseFixture _fixture = new();


    [Fact(DisplayName = "Register user with valid input data")]
    public async void RegisterUser_ValidInputData()
    {
        _fixture.CreateUserTable();

        RegisterUserCommandHandler sut = new(_fixture.DataAccess);
        RegisterUserCommand command = new("name", "password", "email@mail.com", "token");

        Result<int> result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);

        var user = _fixture.DataContext.GetTable("users").GetRowById(1);
        Assert.False(Convert.ToBoolean(user["emailConfirmed"]));
        Assert.Equal("token", user["verifyToken"]);
    }


    [Fact(DisplayName = "Register user with already exists user name")]
    public async void RegisterUser_AlreadyExistsName()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"));

        RegisterUserCommandHandler sut = new(_fixture.DataAccess);
        RegisterUserCommand command = new("name1", "password", "email@mail.com", "token");

        Result<int> result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailed);
        Assert.Equal("User already exists!", result.Errors.First().Message);
    }


    [Fact(DisplayName = "Register user with already exists user email")]
    public async void RegisterUser_AlreadyExistsEmail()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("email", "email1"));

        RegisterUserCommandHandler sut = new(_fixture.DataAccess);
        RegisterUserCommand command = new("name1", "password", "email1", "token");

        Result<int> result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailed);
        Assert.Equal("User already exists!", result.Errors.First().Message);
    }
}