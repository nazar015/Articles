using MediatR;
using Users.Application.Repositories;

namespace Users.Application.Commands.DeleteReader;

public class DeleteReaderHandler(IReaderRepository repository) : IRequestHandler<DeleteReaderCommand, Unit>
{
    public async Task<Unit> Handle(DeleteReaderCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}