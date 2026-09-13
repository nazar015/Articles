using Articles.Api.Models;
using Shared.Exceptions;
using System.Text.Json;

namespace Articles.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = exception switch
            {
                EntityNotFoundException ex => new ErrorResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Errors = ex.Message,
                    TraceId = context.TraceIdentifier
                },
                ValidationException ex => new ErrorResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Errors = ex.Errors,
                    TraceId = context.TraceIdentifier
                },
                _ => new ErrorResponse
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Errors = exception.Message,
                    TraceId = context.TraceIdentifier
                }
            };
            
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            }));
        }
    }
}