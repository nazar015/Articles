using Articles.Api.Models;
using Articles.Application.Queries.GetArticleById;
using MediatR;
using Articles.Application.Commands.CreateArticle;
using Articles.Application.Commands.DeleteArticle;
using Articles.Application.Commands.UpdateArticle;
using Articles.Application.Queries.GetArticles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace Articles.Api.Endpoints;

public static class ArticleEndpoints
{
    public static void MapArticleEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/articles").WithTags("Articles");
        
        group.MapGet("/", async ([AsParameters]GetArticles parameters, [FromServices]IMediator mediator) =>
        {
            var query = new GetArticlesQuery(parameters.AuthorId, parameters.CategoryId, parameters.SearchText);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapGet("/{id}", async ([FromRoute]string id, [FromServices]IMediator mediator) =>
        {
            var query = new GetArticleByIdQuery(id);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapPost("/", async ([FromBody]CreateArticleCommand command, [FromServices]IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Created($"articles/{result}", new { id = result });
        });

        group.MapPut("/", async ([FromBody] UpdateArticleCommand command, [FromServices] IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return Results.Ok(result);
        });

        group.MapDelete("/{id}", async ([FromRoute] string id, [FromServices] IMediator mediator) =>
        {
            var command = new DeleteArticleCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        });
    }
}
