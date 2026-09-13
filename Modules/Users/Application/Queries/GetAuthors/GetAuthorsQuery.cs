using MediatR;
using Users.Application.Dtos;

namespace Users.Application.Queries.GetAuthors;

public record GetAuthorsQuery(string? SearchText = null) : IRequest<IEnumerable<AuthorDto>>;