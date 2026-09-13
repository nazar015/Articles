using MediatR;
using Users.Application.Repositories;
using Users.Domain.Entities;

namespace Users.Application.Commands.CreateAuthor;

public class CreateAuthorHandler(IAuthorRepository repository): IRequestHandler<CreateAuthorCommand, string>
{
    public async Task<string> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var model = new Author()
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            SubscriptionFee = request.SubscriptionFee,
        };
        
        await repository.CreateAsync(model);
        return model.Id;
    }
}