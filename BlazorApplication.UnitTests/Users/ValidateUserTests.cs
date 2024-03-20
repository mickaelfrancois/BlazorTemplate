using Blazor.Application.Users.Commands.ValidateUser;

namespace BlazorApplication.UnitTests.Users;

[Collection("Sequential")]
public class ValidateUserTests()
{
    private readonly DatabaseFixture _fixture = new();


    [Fact(DisplayName = "Validate user confirmation with valid input data")]
    public async void ValidateUser_ValidInputData()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"),
                       new ColumnValueModel("emailConfirmed", false),
                       new ColumnValueModel("verifyToken", "token"));

        ValidateUserCommandHandler sut = new(_fixture.DataAccess);
        ValidateUserCommand command = new(1);

        Result result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        var user = _fixture.DataContext.GetTable("users").GetRowById(1);
        Assert.True(Convert.ToBoolean(user["emailConfirmed"]));
        Assert.Equal(DBNull.Value, user["verifyToken"]);
    }
}