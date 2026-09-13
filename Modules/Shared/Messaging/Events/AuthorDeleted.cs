namespace Shared.Messaging.Events;

public record AuthorDeleted(string UserId, DateTime DeletedAt);