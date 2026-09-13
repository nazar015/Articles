namespace Shared.Persistence;

public record CosmosDbOptions
{
    public required string ConnectionString { get; set; }
    public required string DatabaseName { get; set; }
    public required Dictionary<string, string> Containers { get; set; }
}