using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Shared.Persistence;

namespace Subscriptions.Infrastructure.Persistence;

public class DatabaseInitHostedService(CosmosClient client, IOptions<CosmosDbOptions> options): IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var database =  await client.CreateDatabaseIfNotExistsAsync(options.Value.DatabaseName);
        await database.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(options.Value.Containers["Subscriptions"], "/id"), 400);
        await database.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(options.Value.Containers["Users"], "/id"), 400);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}