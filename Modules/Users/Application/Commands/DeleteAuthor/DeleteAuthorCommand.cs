using MediatR;

namespace Users.Application.Commands.DeleteAuthor;

public record DeleteAuthorCommand(string Id) : IRequest<Unit>;