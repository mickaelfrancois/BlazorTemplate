namespace SqliteFrameworkForUnitTests;

public class ColumnValueModel
{
    public string Name { get; init; }

    public object Value { get; init; }

    public ColumnValueModel(string columnName, object value)
    {
        Name = columnName;
        Value = value;
    }
}
