using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using Users.Application.Messaging;

namespace Users.Infrastructure.Messaging;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceBus(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<ServiceBusOptions>(configuration.GetSection("ServiceBus"));

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;

            var client = new ServiceBusClient(options.ConnectionString);

            return client;
        });

        services.AddSingleton<IUserEventPublisher, UserEventPublisher>();

        return services;
    }
}