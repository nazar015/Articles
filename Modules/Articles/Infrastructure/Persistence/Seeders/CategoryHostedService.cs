using Articles.Domain.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using Shared.Persistence;
using System.Linq;

namespace Articles.Infrastructure.Persistence.Seeders;

public class CategoryHostedService(CosmosClient client, IOptions<CosmosDbOptions> options): IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var database =  await client.CreateDatabaseIfNotExistsAsync(options.Value.DatabaseName);

        var container = await database.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(options.Value.Containers["Categories"], "/id"), 400);

        List<Category> seededCategories = [];
        using var iterator = container.Container.GetItemLinqQueryable<Category>().ToFeedIterator();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            seededCategories.AddRange(response.Resource);
        }
        
        var categoriesToSeed = Seed().ExceptBy(seededCategories.Select(_ => _.Name), category => category.Name);
        
        foreach (var category in categoriesToSeed)
        {
            await container.Container.UpsertItemAsync(category);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
    private List<Category> Seed()
    {
        return new List<Category>
        {
            new Category
            {
                Id = Guid.NewGuid().ToString(),
                CategoryId = 1,
                Name = "Electronics"
            },
            new Category
            {
                Id = Guid.NewGuid().ToString(),
                CategoryId = 2,
                Name = "Clothing"
            },
            new Category
            {
                Id = Guid.NewGuid().ToString(),
                CategoryId = 3,
                Name = "Books"
            },
            new Category
            {
                Id = Guid.NewGuid().ToString(),
                CategoryId = 4,
                Name = "Home & Kitchen"
            },
            new Category
            {
                Id = Guid.NewGuid().ToString(),
                CategoryId = 5,
                Name = "Sports"
            }
        };
    }
}