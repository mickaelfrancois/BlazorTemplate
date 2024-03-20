using Blazor.Application.Security;
using Blazor.Application.Users.Commands.UpdatePassword;

namespace BlazorApplication.UnitTests.Users;

[Collection("Sequential")]
public class UpdatePasswordTests()
{
    private readonly DatabaseFixture _fixture = new();


    [Fact(DisplayName = "Update password with valid input data")]
    public async void UpdatePassword_ValidInputData()
    {
        _fixture.CreateUserTable()
           .AddRow(1, new ColumnValueModel("name", "name1"), new ColumnValueModel("password", SecurityService.GetSha256Hash("0000")));

        UpdatePasswordCommandHandler sut = new(_fixture.DataAccess);
        UpdatePasswordCommand command = new(1, "0000", "1111");

        Result result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);

        var user = _fixture.DataContext.GetTable("users").GetRowById(1);
        string newPasswordHash = SecurityService.GetSha256Hash("1111");
        Assert.Equal(newPasswordHash, user["password"]);
    }


    [Fact(DisplayName = "Update password with invalid old password")]
    public async void UpdatePassword_InvalidPassword()
    {
        _fixture.CreateUserTable()
           .AddRow(1, new ColumnValueModel("name", "name1"), new ColumnValueModel("password", SecurityService.GetSha256Hash("0000")));

        UpdatePasswordCommandHandler sut = new(_fixture.DataAccess);
        UpdatePasswordCommand command = new(1, "1111", "1111");

        Result result = await sut.Handle(command, CancellationToken.None);

        Assert.True(result.IsFailed);
        Assert.Equal("Current password does not match!", result.Errors.First().Message);
    }
}