namespace SqliteFrameworkForUnitTests;

public class Column
{
    public string Name { get; set; } = string.Empty;

    public EColumnType Type { get; set; }

    public bool IsPrimaryKey { get; set; }

    public Column(string name, EColumnType type, bool isPrimaryKey)
    {
        Name = name;
        Type = type;
        IsPrimaryKey = isPrimaryKey;
    }

    public Column(string name, EColumnType type)
    {
        Name = name;
        Type = type;
    }
}
