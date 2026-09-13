using MediatR;
using Users.Application.Repositories;
using Users.Domain.Entities;

namespace Users.Application.Commands.CreateReader;

public class CreateReaderHandler(IReaderRepository repository) : IRequestHandler<CreateReaderCommand, string>
{
    public async Task<string> Handle(CreateReaderCommand request, CancellationToken cancellationToken)
    {
        var reader = new Reader()
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            Login = request.Email,
        };
        
        await repository.CreateAsync(reader);

        return reader.Id;
    }
}