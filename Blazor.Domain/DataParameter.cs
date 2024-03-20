namespace Blazor.Domain;

public class DataParameter
{
    public string Name { get; init; }

    public object Value { get; init; }

    public DataParameter(string name, object value)
    {
        Guard.Against.NullOrEmpty(name);

        Name = name;
        Value = value;
    }
}