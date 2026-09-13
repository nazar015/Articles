namespace Shared.Messaging.Events;

public record ReaderDeleted(string UserId, DateTime DeletedAt);