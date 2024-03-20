using Blazor.Application.Roles.Models;
using System.Diagnostics;

namespace BlazorApplication.UnitTests.Fixtures;


public class DatabaseFixture : IDisposable
{
    public DataContextHelper DataContext { get; init; }

    public DataAccess DataAccess { get; init; }

    private readonly SqliteConnection _connection;


    public DatabaseFixture()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        QueryFactory queryFactory = new(_connection, new SqliteCompiler())
        {
            Logger = compiled =>
            {
                Debug.WriteLine(compiled.ToString());
            }
        };

        DataAccess = new(_connection, queryFactory);
        DataContext = new DataContextHelper(DataAccess);

        DataAccess.Open();
    }


    public DataTable CreateUserTable()
    {
        return DataContext.NewTable(UserDto.EntityName)
            .AddPrimaryKey("id")
            .AddColumn("name", EColumnType.Varchar)
            .AddColumn("password", EColumnType.Varchar)
            .AddColumn("email", EColumnType.Varchar)
            .AddColumn("lockoutEnabled", EColumnType.Bool)
            .AddColumn("emailConfirmed", EColumnType.Bool)
            .AddColumn("verifyToken", EColumnType.Varchar)
            .CreateTable();
    }


    public DataTable CreateUserRolesTable()
    {
        return DataContext.NewTable(UserRolesDto.EntityName)
            .AddPrimaryKey("id")
            .AddColumn("userId", EColumnType.Int)
            .AddColumn("roleId", EColumnType.Int)
            .CreateTable();
    }


    public DataTable CreateRolesTable()
    {
        return DataContext.NewTable(RoleDto.EntityName)
            .AddPrimaryKey("id")
            .AddColumn("name", EColumnType.Varchar)            
            .CreateTable();
    }


    public void Dispose()
    {
        _connection.Dispose();

        GC.SuppressFinalize(this);
    }
}
