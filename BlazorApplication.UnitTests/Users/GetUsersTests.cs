using Blazor.Application.Users.Queries.GetUsers;

namespace BlazorApplication.UnitTests.Users;

[Collection("Sequential")]
public class GetUsersTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture = fixture;


    [Fact(DisplayName = "Get users, should get full list of users")]
    public async void GetUsers()
    {
        _fixture.CreateUserTable()
            .AddRow(1, new ColumnValueModel("name", "name1"))
            .AddRow(2, new ColumnValueModel("name", "name2"));

        GetUsersQueryHandler sut = new(_fixture.DataAccess);
        GetUsersQuery query = new();

        IEnumerable<UserDto> result = await sut.Handle(query, CancellationToken.None);
        List<UserDto> users = result.ToList();

        Assert.Equal(2, users.Count);
        Assert.Collection(users,
            e => Assert.Equal("name1", e.Name),
            e => Assert.Equal("name2", e.Name));
    }
}