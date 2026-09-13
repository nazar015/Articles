using MediatR;
using Users.Application.Dtos;
using Users.Application.Repositories;

namespace Users.Application.Queries.GetReaders;

public class GetReadersHandler(IReaderRepository repository): IRequestHandler<GetReadersQuery, IEnumerable<ReaderDto>>
{
    public async Task<IEnumerable<ReaderDto>> Handle(GetReadersQuery request, CancellationToken cancellationToken)
    {
        var records = await repository.GetAsync(request.SearchText);
        var results = records.Select(x => new ReaderDto(x.Id, x.Login, x.Email));
        return results;
    }
}