using MediatR;
using Users.Application.Dtos;
using Users.Application.Repositories;

namespace Users.Application.Queries.GetAuthors;

public class GetAuthorsHandler(IAuthorRepository repository) : IRequestHandler<GetAuthorsQuery, IEnumerable<AuthorDto>>
{
    public async Task<IEnumerable<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var objects = await repository.GetAsync(request.SearchText);
        var results = objects.Select(x => new AuthorDto(x.Id, x.FirstName, x.LastName, x.Email, x.SubscriptionFee));
        return results;
    }
}