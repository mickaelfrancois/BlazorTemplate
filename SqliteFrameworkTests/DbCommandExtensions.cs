using System.Data;

namespace SqliteFrameworkForUnitTests;

public static class DbCommandExtensions
{
    public static IDbCommand AddParameter(this IDbCommand command, string name, object? value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);

        return command;
    }
}
