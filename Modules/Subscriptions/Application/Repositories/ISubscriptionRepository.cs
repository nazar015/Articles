using Subscriptions.Domain.Entities;

namespace Subscriptions.Application.Repositories;

public interface ISubscriptionRepository
{
    Task CreateAsync(Subscription subscription);
    Task<bool> DeleteAsync(string id);
    public Task DeleteByReaderAsync(string id);
    public Task DeleteByAuthorAsync(string id);
    Task<IEnumerable<Subscription>> GetAsync(string searchText);
    Task CreateUserAsync(string id, string type);
    Task DeleteUserAsync(string id);
}