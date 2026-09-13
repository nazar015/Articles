namespace Subscriptions.Domain.Entities;

public record User
{
    public string Id { get; set; }
    public string Type { get; set; }
}