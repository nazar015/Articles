namespace Articles.Domain.Entities;

public record Article
{
    public string Id { get; set; }
    public string Image { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string AuthorId { get; set; }
    public string CategoryId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}
