namespace Blazor.Domain;

public class DataAccess(IDbConnection connection, QueryFactory queryFactory)
{
    private readonly IDbConnection _connection = Guard.Against.Null(connection);

    public QueryFactory QueryFactory { get; init; } = Guard.Against.Null(queryFactory);


    public void Open()
    {
        if (_connection.State == ConnectionState.Closed)
            _connection.Open();
    }


    public void Close()
    {
        if (_connection.State == ConnectionState.Open)
            _connection.Close();
    }


    public IDataReader ExecuteReader(IDbCommand command)
    {
        Guard.Against.Null(command);

        bool wasClosed = _connection.State == ConnectionState.Closed;

        try
        {
            if (wasClosed)
                _connection.Open();

            return command.ExecuteReader();
        }
        finally
        {
            if (wasClosed)
                _connection.Close();
        }
    }


    public IDataReader ExecuteQuery(string query, IEnumerable<DataParameter>? parameters = null)
    {
        Guard.Against.Null(query);

        bool wasClosed = _connection.State == ConnectionState.Closed;

        IDbCommand? command = null;

        try
        {
            command = GetCommand(query, parameters);

            if (wasClosed)
                _connection.Open();

            return command.ExecuteReader();
        }
        finally
        {
            command?.Dispose();

            if (wasClosed)
                _connection.Close();
        }
    }


    public int ExecuteNonQuery(IDbCommand command)
    {
        Guard.Against.Null(command);

        bool wasClosed = _connection.State == ConnectionState.Closed;

        try
        {
            if (wasClosed)
                _connection.Open();

            return command.ExecuteNonQuery();
        }
        finally
        {
            if (wasClosed)
                _connection.Close();
        }
    }


    public int ExecuteNonQuery(string query, IEnumerable<DataParameter>? parameters = null)
    {
        Guard.Against.Null(query);

        bool wasClosed = _connection.State == ConnectionState.Closed;

        IDbCommand? command = null;

        try
        {
            command = GetCommand(query, parameters);

            if (wasClosed)
                _connection.Open();

            return command.ExecuteNonQuery();
        }
        finally
        {
            command?.Dispose();

            if (wasClosed)
                _connection.Close();
        }
    }


    public object? ExecuteScalar(IDbCommand command)
    {
        Guard.Against.Null(command);

        bool wasClosed = _connection.State == ConnectionState.Closed;

        try
        {
            if (wasClosed)
                _connection.Open();

            return command.ExecuteScalar();
        }
        finally
        {
            if (wasClosed)
                _connection.Close();
        }
    }


    public object? ExecuteScalar(string query, IEnumerable<DataParameter>? parameters = null)
    {
        Guard.Against.Null(query);

        bool wasClosed = _connection.State == ConnectionState.Closed;

        IDbCommand? command = null;

        try
        {
            command = GetCommand(query, parameters);

            if (wasClosed)
                _connection.Open();

            return command.ExecuteScalar();
        }
        finally
        {
            command?.Dispose();

            if (wasClosed)
                _connection.Close();
        }
    }


    public void BeginTransaction()
    {
        using IDbCommand command = CreateCommand("BEGIN");

        ExecuteNonQuery(command);
    }


    public void CommitTransaction()
    {
        using IDbCommand command = CreateCommand("COMMIT");

        ExecuteNonQuery(command);
    }


    public void RollbackTransaction()
    {
        using IDbCommand command = CreateCommand("ROLLBACK");

        ExecuteNonQuery(command);
    }


    public IDbCommand CreateCommand(string query)
    {
        IDbCommand command = _connection.CreateCommand();
        command.CommandText = query;

        return command;
    }


    private IDbCommand GetCommand(string query, IEnumerable<DataParameter>? parameters = null)
    {
        IDbCommand command = _connection.CreateCommand();
        command.CommandText = query;

        if (parameters != null)
        {
            foreach (var param in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = param.Name;
                parameter.Value = param.Value;
            }

            command.Parameters.Add(parameters);
        }

        return command;
    }
}
