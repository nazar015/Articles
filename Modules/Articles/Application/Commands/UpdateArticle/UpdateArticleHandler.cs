using Articles.Application.Dtos;
using Articles.Application.Repositories;
using Articles.Domain.Entities;
using MediatR;

namespace Articles.Application.Commands.UpdateArticle;

public class UpdateArticleHandler(IArticleRepository repository) : IRequestHandler<UpdateArticleCommand, ArticleDto>
{
    public async Task<ArticleDto> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        var updated = await repository.UpdateAsync(new Article()
        {
            Id = request.Id,
            Image = request.Image,
            Title = request.Title,
            Price = request.Price,
            AuthorId = request.AuthorId,
            CategoryId = request.CategoryId
        });
        
        return new ArticleDto(updated.Id,  updated.Image, updated.Title, updated.Price, updated.AuthorId, updated.CategoryId);
    }
}