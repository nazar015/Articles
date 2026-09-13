namespace Subscriptions.Domain.Entities;

public record Subscription
{
    public string Id { get; set; }
    public string ReaderId { get; set; }
    public string AuthorId { get; set; }
}