using Users.Domain.Entities;

namespace Users.Application.Repositories;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAsync(string searchText);
    Task CreateAsync(Author item);
    Task<bool> DeleteAsync(string id);
}