using Articles.Application.Dtos;
using MediatR;

namespace Articles.Application.Commands.UpdateArticle;

public record UpdateArticleCommand : IRequest<ArticleDto>
{
    public string Id { get; set; }
    public string Image { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string AuthorId { get; set; }
    public string CategoryId { get; set; }
}