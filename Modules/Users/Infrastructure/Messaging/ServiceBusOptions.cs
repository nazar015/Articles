namespace Users.Infrastructure.Messaging;

public record ServiceBusOptions
{
    public required string ConnectionString { get; set; }
    public required Dictionary<string, string> Topics { get; set; }
}