namespace Shared.Exceptions;

public class EntityNotFoundException : Exception
{
    public string? EntityType { get; }
    public object? Key { get; }
    
    public EntityNotFoundException(string? entityType, object? key) : base($"Entity of type '{entityType}' with key '{key}' was not found.")
    {
        EntityType = entityType;
        Key = key;
    }
}