using Blazor.Domain;

namespace SqliteFrameworkForUnitTests;

public class DataContextHelper(DataAccess dataAccess)
{
    private readonly DataAccess _dataAccess = dataAccess;

    private readonly Dictionary<string, DataTable> _tables = [];

    public DataTable NewTable(string tableName)
    {
        var table = new DataTable(tableName, _dataAccess);

        if (_tables.TryAdd(key: tableName, value: table) == false)
            throw new InvalidOperationException($"Table '{tableName}' already added");

        return table;
    }


    public DataTable GetTable(string tableName)
    {
        if (_tables.TryGetValue(tableName, out var table))
            return table;

        throw new InvalidOperationException($"Table '{tableName}' was not found");
    }
}
