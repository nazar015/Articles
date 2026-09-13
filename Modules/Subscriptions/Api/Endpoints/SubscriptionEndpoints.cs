using MediatR;
using Microsoft.AspNetCore.Mvc;
using Subscriptions.Api.Models;
using Subscriptions.Application.Commands.CreateSubscription;
using Subscriptions.Application.Commands.DeleteSubscription;
using Subscriptions.Application.Queries.GetSubscriptions;
using Shared.Exceptions;

namespace Subscriptions.Api.Endpoints;

public static class SubscriptionEndpoints
{
    public static void MapSubscriptionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/subscriptions").WithTags("Subscriptions");

        group.MapGet("/", async (string? searchText, [FromServices] IMediator mediator) =>
        {
            var query = new GetSubscriptionsQuery(searchText);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapPost("/{id}", async ([FromRoute] string id, [FromBody] CreateSubscriptionRequest body, [FromServices] IMediator mediator) =>
        {
            try
            {
                var command = new CreateSubscriptionCommand(body.ReaderId, body.AuthorId);
                var result = await mediator.Send(command);
                return Results.Created($"/subscriptions/{result}", new { id = result });
            }
            catch (EntityNotFoundException e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        group.MapDelete("/{id}", async ([FromRoute] string id, [FromServices] IMediator mediator) =>
        {
            var command = new DeleteSubscriptionCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        });
    }
}