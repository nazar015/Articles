namespace Users.Domain.Entities;

public record Author
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public decimal SubscriptionFee { get; set; }
}