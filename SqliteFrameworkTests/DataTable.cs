using Blazor.Domain;
using System.Data;
using System.Text;

namespace SqliteFrameworkForUnitTests;

public class DataTable(string tableName, DataAccess dataAccess)
{
    public string TableName { get; init; } = tableName;

    public List<Column> Columns { get; init; } = [];

    private string _primaryKeyField = string.Empty;



    public DataTable AddPrimaryKey(string columnName)
    {
        Columns.Add(new Column(columnName, EColumnType.Int, true));
        _primaryKeyField = columnName;

        return this;
    }


    public DataTable AddColumn(string columnName, EColumnType type)
    {
        Columns.Add(new Column(columnName, type));

        return this;
    }


    public DataTable AddRow(int id, params ColumnValueModel[] values)
    {
        string query = $"INSERT INTO `{TableName}` (`{_primaryKeyField}`";
        string separator = ",";

        for (int i = 0; i < values.Length; i++)
        {
            query += $"{separator}`{values[i].Name}`";
        }

        separator = ",";
        query += $") VALUES (@{_primaryKeyField}";
        for (int i = 0; i < values.Length; i++)
        {
            query += $"{separator}@{values[i].Name}";
        }

        query += ")";

        using IDbCommand command = dataAccess.CreateCommand(query);

        command.AddParameter($"@{_primaryKeyField}", id);

        for (int i = 0; i < values.Length; i++)
        {
            command.AddParameter($"@{values[i].Name}", values[i].Value);
        }

        dataAccess.ExecuteNonQuery(command);

        return this;
    }


    public DataTable AddRow(params ColumnValueModel[] values)
    {
        string query = $"INSERT INTO `{TableName}` (";
        string separator = "";

        for (int i = 0; i < values.Length; i++)
        {
            query += $"{separator}`{values[i].Name}`";
            separator = ",";
        }

        separator = "";
        query += ") VALUES (";
        for (int i = 0; i < values.Length; i++)
        {
            query += $"{separator}@{values[i].Name}";
            separator = ",";
        }

        query += ")";

        using IDbCommand command = dataAccess.CreateCommand(query);

        for (int i = 0; i < values.Length; i++)
        {
            command.AddParameter($"@{values[i].Name}", values[i].Value);
        }

        dataAccess.ExecuteNonQuery(command);

        return this;
    }


    public DataTable CreateTable()
    {
        string query = GetCreateTableQuery();

        using var command = dataAccess.CreateCommand(query);
        dataAccess.ExecuteNonQuery(command);

        return this;
    }


    private string GetCreateTableQuery()
    {
        var query = new StringBuilder();
        string separator = "";

        query.Append($"CREATE TABLE `{TableName}` (");

        foreach (Column column in Columns)
        {
            string columnType = GetColumnDataTypeForDatabase(column.Type);
            query.Append($"{separator}`{column.Name}` {columnType}" + (column.IsPrimaryKey ? " PRIMARY KEY " : ""));
            separator = ",";
        }

        query.Append(')');

        return query.ToString();
    }


    public int GetRowCount()
    {
        string query = $"SELECT COUNT(*) FROM `{TableName}`";

        using IDbCommand command = dataAccess.CreateCommand(query);

        return Convert.ToInt32(dataAccess.ExecuteScalar(command));
    }


    public Dictionary<string, object> GetRowById(int id)
    {
        Dictionary<string, object> result = [];

        string query = $"SELECT * FROM `{TableName}` WHERE `{_primaryKeyField}` = @value";

        using IDbCommand command = dataAccess.CreateCommand(query);
        command.AddParameter("@value", id);

        using IDataReader reader = dataAccess.ExecuteReader(command);

        if (reader.Read())
        {
            object[] values = new object[reader.FieldCount];
            reader.GetValues(values);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                object value = reader.GetValue(i);

                result.Add(name, value);
            }
        }

        return result;
    }


    public static string GetColumnDataTypeForDatabase(EColumnType columnType)
    {
        return columnType switch
        {
            EColumnType.Int => "integer",
            EColumnType.Bool => "TINYINT(1)",
            EColumnType.Varchar => "VARCHAR(255)",
            EColumnType.DateTime => "DATETIME",
            _ => throw new ArgumentOutOfRangeException($"Column type {columnType} has no mapping"),
        };
    }
}
