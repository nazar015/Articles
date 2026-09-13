using MediatR;
using Articles.Domain.Entities;
namespace Articles.Application.Commands.CreateArticle;

public record CreateArticleCommand : IRequest<string>
{
    public string Image { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string AuthorId { get; set; }
    public string CategoryId { get; set; }
}
