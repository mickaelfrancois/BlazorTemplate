using Blazor.Application.Users.Queries.GetUserById;

namespace BlazorApplication.UnitTests.Users;

[Collection("Sequential")]
public class GetUserByIdTests()
{
    private readonly DatabaseFixture _fixture = new();


    [Fact(DisplayName = "Get user by id with valid ID, should get valid user")]
    public async void GetUserById_ValidId()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"), new ColumnValueModel("lockoutEnabled", true))
            .AddRow(2, new ColumnValueModel("name", "name2"));
        _fixture.CreateUserRolesTable();
        _fixture.CreateRolesTable();

        GetUserByIdQueryHandler sut = new(_fixture.DataAccess);
        GetUserByIdQuery query = new(1);

        Result<UserVm> result = await sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.Id);
        Assert.Equal("name1", result.Value.Name);
        Assert.True(result.Value.LockoutEnabled);
    }


    [Fact(DisplayName = "Get user by id with an unknow ID, should return null")]
    public async void GetUserById_UnknowId()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"), new ColumnValueModel("lockoutEnabled", true));
        _fixture.CreateUserRolesTable();
        _fixture.CreateRolesTable();


        GetUserByIdQueryHandler sut = new(_fixture.DataAccess);
        GetUserByIdQuery query = new(999);

        Result<UserVm> result = await sut.Handle(query, CancellationToken.None);

        Assert.True(result.IsFailed);
    }
}