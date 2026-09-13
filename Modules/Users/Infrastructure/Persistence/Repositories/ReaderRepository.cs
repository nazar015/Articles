using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using Users.Application.Repositories;
using Users.Domain.Entities;
using Shared.Persistence;

namespace Users.Infrastructure.Persistence.Repositories;

public class ReaderRepository(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options) : IReaderRepository
{
    private readonly Container _readers = cosmosClient.GetContainer(options.Value.DatabaseName, options.Value.Containers["Readers"]);
    
    public async Task<IEnumerable<Reader>> GetAsync(string searchText)
    {
        IQueryable<Reader> queryable =  _readers.GetItemLinqQueryable<Reader>();
        
        if (!string.IsNullOrEmpty(searchText))
        {
            queryable = queryable.Where(a => a.Id.Contains(searchText) || a.Login.Contains(searchText) || 
                                             a.Email.Contains(searchText));
        }
        
        using var iterator = queryable.ToFeedIterator();

        var results = new List<Reader>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.Resource);
        }

        return results;
    }

    public async Task CreateAsync(Reader item)
    {
        var result =  await _readers.CreateItemAsync(item, new PartitionKey(item.Id));
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deleted = await _readers.DeleteItemAsync<Author>(id, new PartitionKey(id));
        return (int)deleted.StatusCode == 200 || (int)deleted.StatusCode == 204;
    }
}