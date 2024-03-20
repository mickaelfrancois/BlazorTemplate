using Blazor.Application.Users.Queries.GetUserByVerirfyToken;

namespace BlazorApplication.UnitTests.Users;

[Collection("Sequential")]
public class GetUserByVerifyTokenTests()
{
    private readonly DatabaseFixture _fixture = new();


    [Fact(DisplayName = "Get user by validation token with a valid token, should get valid user")]
    public async void GetUserByVerifyToken_ValidId()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"), new ColumnValueModel("verifytoken", "token"))
            .AddRow(2, new ColumnValueModel("name", "name2"));
        _fixture.CreateUserRolesTable();
        _fixture.CreateRolesTable();

        GetUserByVerifyTokenQueryHandler sut = new(_fixture.DataAccess);
        GetUserByVerifyTokenQuery query = new("token");

        Result<UserDto> result = await sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.Id);
        Assert.Equal("name1", result.Value.Name);
    }


    [Fact(DisplayName = "Get user by validation token with an unknown token, should return null")]
    public async void GetUserById_UnknowId()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"), new ColumnValueModel("verifytoken", "token"));
        _fixture.CreateUserRolesTable();
        _fixture.CreateRolesTable();


        GetUserByVerifyTokenQueryHandler sut = new(_fixture.DataAccess);
        GetUserByVerifyTokenQuery query = new("Invalid");

        Result<UserDto> result = await sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsFailed);
    }
}