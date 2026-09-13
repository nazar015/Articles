using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using Subscriptions.Application.Repositories;
using Subscriptions.Domain.Entities;
using Shared.Exceptions;
using User = Subscriptions.Domain.Entities.User;
using Shared.Persistence;

namespace Subscriptions.Infrastructure.Persistence.Repositories;

public class SubscriptionRepository(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options) : ISubscriptionRepository
{
    private readonly Container _subscriptions = cosmosClient.GetContainer(options.Value.DatabaseName, options.Value.Containers["Subscriptions"]);
    private readonly Container _users = cosmosClient.GetContainer(options.Value.DatabaseName, options.Value.Containers["Users"]);
    
    public async Task<IEnumerable<Subscription>> GetAsync(string searchText)
    {
        IQueryable<Subscription> queryable =  _subscriptions.GetItemLinqQueryable<Subscription>();
        
        if (!string.IsNullOrEmpty(searchText))
        {
            queryable = queryable.Where(a => a.Id.Contains(searchText) || a.ReaderId.Contains(searchText) || 
                                             a.AuthorId.Contains(searchText));
        }
        
        using var iterator = queryable.ToFeedIterator();

        var results = new List<Subscription>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.Resource);
        }

        return results;
    }

    public async Task CreateAsync(Subscription item)
    {
        var query = new QueryDefinition("""
                                        SELECT * FROM c 
                                        WHERE (c.id = @authorId AND c.type = @authorType)
                                           OR (c.id = @readerId AND c.type = @readerType)
                                        """)
            .WithParameter("@authorId", item.AuthorId)
            .WithParameter("@authorType", "author")
            .WithParameter("@readerId", item.ReaderId)
            .WithParameter("@readerType", "reader");

        var iterator = _users.GetItemQueryIterator<User>(query);

        var results = new List<User>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }

        if (results.Any(_ =>  _.Id == item.ReaderId))
        {
            throw new EntityNotFoundException(nameof(User), item.ReaderId);
        }
            
        if (results.Any(_ =>  _.Id == item.AuthorId))
        {
            throw new EntityNotFoundException(nameof(User), item.AuthorId);
        }
        
        var result =  await _subscriptions.CreateItemAsync(item, new PartitionKey(item.Id));
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deleted = await _subscriptions.DeleteItemAsync<Subscription>(id, new PartitionKey(id));
        return (int)deleted.StatusCode == 200 || (int)deleted.StatusCode == 204;
    }
    
    public async Task DeleteByReaderAsync(string id)
    {
        var subscriptions = _subscriptions
            .GetItemLinqQueryable<Subscription>(allowSynchronousQueryExecution: false)
            .Where(s => s.ReaderId == id)
            .ToFeedIterator();

        while (subscriptions.HasMoreResults)
        {
            foreach (var item in await subscriptions.ReadNextAsync())
            {
                await _subscriptions.DeleteItemAsync<Subscription>(
                    item.Id,
                    new PartitionKey(item.Id));
            }
        }
    }
    
    public async Task DeleteByAuthorAsync(string id)
    {
        var subscriptions = _subscriptions
            .GetItemLinqQueryable<Subscription>(allowSynchronousQueryExecution: false)
            .Where(s => s.AuthorId == id)
            .ToFeedIterator();

        while (subscriptions.HasMoreResults)
        {
            foreach (var item in await subscriptions.ReadNextAsync())
            {
                await _subscriptions.DeleteItemAsync<Subscription>(
                    item.Id,
                    new PartitionKey(item.Id));
            }
        }
    }

    public async Task CreateUserAsync(string id, string type)
    {
        var user = new User() { Id = id, Type = type };
        await _users.CreateItemAsync(user, new PartitionKey(user.Id));
    }
    
    public async Task DeleteUserAsync(string id)
    {
        await _users.DeleteItemAsync<User>(id, new PartitionKey(id));
    }
}