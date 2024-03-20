using Blazor.Application.Security;
using Blazor.Application.Users.Queries.GetUserWithCredentials;

namespace BlazorApplication.UnitTests.Users;

[Collection("Sequential")]
public class GetUserWithCredentialsTests()
{
    private readonly DatabaseFixture _fixture = new();


    [Fact(DisplayName = "Get user with credentials with valid credentials, should get valid user")]
    public async void GetUserWithCredentials_ValidCredentials()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"), new ColumnValueModel("password", SecurityService.GetSha256Hash("0000")))
            .AddRow(2, new ColumnValueModel("name", "name2"));

        GetUserWithCredentialsQueryHandler sut = new(_fixture.DataAccess);
        GetUserWithCredentialsQuery query = new("name1", "0000");

        UserDto? result = await sut.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("name1", result.Name);
    }


    [Fact(DisplayName = "Get user by id with invalid credentials, should return null")]
    public async void GetUserWithCredentials_InvalidCredentials()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"), new ColumnValueModel("password", SecurityService.GetSha256Hash("0000")));

        GetUserWithCredentialsQueryHandler sut = new(_fixture.DataAccess);
        GetUserWithCredentialsQuery query = new("name1", "invalid");

        UserDto? result = await sut.Handle(query, CancellationToken.None);

        Assert.Null(result);
    }
}