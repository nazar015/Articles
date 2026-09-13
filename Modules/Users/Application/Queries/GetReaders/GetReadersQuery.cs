using MediatR;
using Users.Application.Dtos;

namespace Users.Application.Queries.GetReaders;

public record GetReadersQuery(string? SearchText = null) : IRequest<IEnumerable<ReaderDto>>;