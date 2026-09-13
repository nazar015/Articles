using Articles.Application.Repositories;
using Articles.Application.Dtos;
using MediatR;

namespace Articles.Application.Queries.GetArticles;

public class GetArticlesHandler(IArticleRepository repository) : IRequestHandler<GetArticlesQuery, IEnumerable<ArticleDto>>
{
    public async Task<IEnumerable<ArticleDto>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        var articles = await repository.GetAsync(request.AuthorId, request.CategoryId, request.SearchText);
        var results = articles.Select(_ => new ArticleDto(_.Id, _.Image, _.Title, _.Price, _.AuthorId, _.CategoryId));
        return results;
    }
}