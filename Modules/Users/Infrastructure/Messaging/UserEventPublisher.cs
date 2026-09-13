using Azure.Messaging.ServiceBus;
using Shared.Messaging.Events;
using Users.Application.Messaging;
using System.Text.Json;

namespace Users.Infrastructure.Messaging;

public class UserEventPublisher: IUserEventPublisher
{
    private readonly ServiceBusClient _client;
    private const string TopicName = "user-events";

    public UserEventPublisher(ServiceBusClient client)
    {
        _client = client;
    }

    public async Task PublishReaderDeletedAsync(string userId, CancellationToken ct = default)
    {
        await using var sender = _client.CreateSender(TopicName);

        var evt = new ReaderDeleted(userId, DateTime.UtcNow);
        var message = new ServiceBusMessage(JsonSerializer.Serialize(evt))
        {
            Subject = "ReaderDeleted",
            ContentType = "application/json"
        };

        await sender.SendMessageAsync(message, ct);
    }

    public async Task PublishAuthorDeletedAsync(string userId, CancellationToken ct = default)
    {
        await using var sender = _client.CreateSender(TopicName);

        var evt = new AuthorDeleted(userId, DateTime.UtcNow);
        var message = new ServiceBusMessage(JsonSerializer.Serialize(evt))
        {
            Subject = "AuthorDeleted",
            ContentType = "application/json"
        };

        await sender.SendMessageAsync(message, ct);
    }
}