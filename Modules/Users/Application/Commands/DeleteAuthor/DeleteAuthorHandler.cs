using MediatR;
using Users.Application.Repositories;

namespace Users.Application.Commands.DeleteAuthor;

public class DeleteAuthorHandler(IAuthorRepository repository) : IRequestHandler<DeleteAuthorCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}