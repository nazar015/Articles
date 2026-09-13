using Articles.Application.Dtos;
using MediatR;

namespace Articles.Application.Queries.GetArticles;

public record GetArticlesQuery(string AuthorId, string CategoryId, string SearchText) : IRequest<IEnumerable<ArticleDto>>;