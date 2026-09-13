using MediatR;

namespace Users.Application.Commands.DeleteReader;

public record DeleteReaderCommand(string Id) : IRequest<Unit>;