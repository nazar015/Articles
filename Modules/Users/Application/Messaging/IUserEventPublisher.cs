namespace Users.Application.Messaging;

public interface IUserEventPublisher
{
    Task PublishReaderDeletedAsync(string userId, CancellationToken ct = default);
    Task PublishAuthorDeletedAsync(string userId, CancellationToken ct = default);
}