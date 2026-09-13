using System.Text.Json;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Shared.Persistence;

namespace Subscriptions.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddCosmosDbContainer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CosmosDbOptions>(configuration.GetSection("CosmosDb"));

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;

            var client = new CosmosClient(options.ConnectionString, new CosmosClientOptions
            {
                ConnectionMode = ConnectionMode.Gateway,
                LimitToEndpoint = true,
                UseSystemTextJsonSerializerWithOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy =  JsonNamingPolicy.CamelCase,
                },
                
                HttpClientFactory = () =>
                {
                    var handler = new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                    };
                    
                    return new HttpClient(handler)
                    {
                        Timeout = TimeSpan.FromSeconds(30)
                    };
                }
            });
            
            return client;
        });
        
        services.AddHostedService<DatabaseInitHostedService>();
        
        return services;
    }
}