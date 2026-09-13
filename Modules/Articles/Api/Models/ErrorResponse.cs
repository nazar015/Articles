namespace Articles.Api.Models;

public record ErrorResponse
{
    public int StatusCode { get; set; }
    public object Errors { get; set; }
    public string TraceId { get; set; }
}