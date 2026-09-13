namespace Articles.Domain.Entities;

public record Category
{
    public string Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
}