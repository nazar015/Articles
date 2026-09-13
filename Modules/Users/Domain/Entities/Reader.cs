namespace Users.Domain.Entities;

public record Reader
{
    public string Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
}