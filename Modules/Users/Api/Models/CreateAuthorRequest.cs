namespace Users.Api.Models;

public record CreateAuthorRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public decimal SubscriptionFee { get; set; }
}