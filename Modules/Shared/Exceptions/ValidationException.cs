namespace Shared.Exceptions;

public class ValidationException : Exception
{
    public string? EntityType { get; }
    
    /// <summary>
    /// properties with validation descriptions
    /// </summary>
    public IReadOnlyDictionary<string, string> Errors { get; }
    
    public ValidationException(string entityType, Dictionary<string, string> propertiesWithDescriptions)
    {
        EntityType = entityType;
        Errors = propertiesWithDescriptions;
    }
}