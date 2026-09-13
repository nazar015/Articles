using MediatR;
using Articles.Application.Dtos;
namespace Articles.Application.Queries.GetArticleById;

public record GetArticleByIdQuery(string Id) : IRequest<ArticleDto>;