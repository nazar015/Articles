using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using Users.Application.Repositories;
using Users.Domain.Entities;
using Shared.Persistence;

namespace Users.Infrastructure.Persistence.Repositories;

public class AuthorRepository(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options): IAuthorRepository
{
    private readonly Container _authors = cosmosClient.GetContainer(options.Value.DatabaseName, options.Value.Containers["Authors"]);
    
    public async Task<IEnumerable<Author>> GetAsync(string searchText)
    {
        IQueryable<Author> queryable =  _authors.GetItemLinqQueryable<Author>();
        
        if (!string.IsNullOrEmpty(searchText))
        {
            queryable = queryable.Where(a => a.Id.Contains(searchText) || a.Email.Contains(searchText) || 
                                             a.FirstName.Contains(searchText) || a.LastName.Contains(searchText));
        }
        
        using var iterator = queryable.ToFeedIterator();

        var results = new List<Author>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.Resource);
        }

        return results;
    }

    public async Task CreateAsync(Author item)
    {
        var result =  await _authors.CreateItemAsync(item, new PartitionKey(item.Id));
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deleted = await _authors.DeleteItemAsync<Author>(id, new PartitionKey(id));
        return (int)deleted.StatusCode == 200 || (int)deleted.StatusCode == 204;
    }
}