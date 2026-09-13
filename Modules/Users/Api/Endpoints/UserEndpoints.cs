using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Models;
using Users.Application.Commands.CreateAuthor;
using Users.Application.Commands.CreateReader;
using Users.Application.Commands.DeleteAuthor;
using Users.Application.Commands.DeleteReader;
using Users.Application.Queries.GetAuthors;
using Users.Application.Queries.GetReaders;

namespace Users.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapAuthorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/authors").WithTags("Users");

        group.MapGet("/", async (string? searchText, [FromServices] IMediator mediator) =>
        {
            var query = new GetAuthorsQuery(searchText);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });
        
        group.MapPost("/", async ([FromBody] CreateAuthorRequest body, [FromServices] IMediator mediator) =>
        {
            var command = new CreateAuthorCommand(body.FirstName, body.LastName, body.Email, body.Password, body.SubscriptionFee);
            var result = await mediator.Send(command);
            return Results.Created($"/authors/{result}", new { id = result });
        });

        group.MapDelete("/{id}", async ([FromRoute] string id, [FromServices] IMediator mediator) =>
        {
            var command = new DeleteAuthorCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        });
    }
    
    public static void MapReaderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/readers").WithTags("Users");
        
        group.MapGet("/", async (string? searchText, [FromServices] IMediator mediator) =>
        {
            var query = new GetReadersQuery(searchText);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapPost("/{id}", async ([FromRoute] string id, [FromBody] CreateReaderRequest body, [FromServices] IMediator mediator) =>
        {
            var command = new CreateReaderCommand(body.Email, body.Password);
            var result = await mediator.Send(command);
            return Results.Created($"/readers/{result}", new { id = result });
        });

        group.MapDelete("/{id}", async ([FromRoute] string id, [FromServices] IMediator mediator) =>
        {
            var command = new DeleteReaderCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        });
    }
}
