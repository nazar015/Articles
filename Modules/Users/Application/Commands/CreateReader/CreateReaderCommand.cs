using MediatR;

namespace Users.Application.Commands.CreateReader;

public record CreateReaderCommand(string Email, string Password) : IRequest<string>;