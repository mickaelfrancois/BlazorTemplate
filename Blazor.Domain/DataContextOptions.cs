namespace Blazor.Domain;

public record DataContextOptions
{
    public string ConnectionString { get; set; } = string.Empty;
}
