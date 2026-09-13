namespace Articles.Domain.Entities;

public record Author
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
}