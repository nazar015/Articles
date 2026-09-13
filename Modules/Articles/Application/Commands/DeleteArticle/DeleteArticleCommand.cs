using Articles.Application.Dtos;
using MediatR;

namespace Articles.Application.Commands.DeleteArticle;

public record DeleteArticleCommand(string Id): IRequest<bool>;