using MediatR;
using Articles.Application.Repositories;
using Articles.Domain.Entities;

namespace Articles.Application.Commands.CreateArticle;

public class CreateArticleHandler(IArticleRepository repository) : IRequestHandler<CreateArticleCommand, string>
{
    /// <returns>Id of a created entity.</returns>
    public async Task<string> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var article = new Article
        {
            Id = Guid.NewGuid().ToString(),
            Image = request.Image,
            Title = request.Title,
            Price = request.Price,
            AuthorId = request.AuthorId,
            CategoryId = request.CategoryId
        };

        await repository.CreateAsync(article);

        return article.Id;
    }
}