using Users.Domain.Entities;

namespace Users.Application.Repositories;

public interface IReaderRepository
{
    Task<IEnumerable<Reader>> GetAsync(string searchText);
    Task CreateAsync(Reader item);
    Task<bool> DeleteAsync(string id);
}