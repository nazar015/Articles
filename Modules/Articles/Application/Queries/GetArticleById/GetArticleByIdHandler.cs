using MediatR;
using Articles.Application.Dtos;
using Articles.Application.Repositories;

namespace Articles.Application.Queries.GetArticleById;

public class GetArticleByIdHandler(IArticleRepository repository) : IRequestHandler<GetArticleByIdQuery, ArticleDto>
{
    public async Task<ArticleDto> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await repository.GetAsync(request.Id)
        ?? throw new KeyNotFoundException($"Article with ID {request.Id} not found.");

        return new ArticleDto(article.Id, article.Image, article.Title, article.Price, article.AuthorId, article.CategoryId);
    }
}
