using Articles.Domain.Entities;

namespace Articles.Application.Repositories;

public interface IArticleRepository
{
    Task<Article> GetAsync(string id);
    Task<IEnumerable<Article>> GetAsync(string authorId, string categoryId, string searchText);
    Task<Article> CreateAsync(Article item);
    Task<Article> UpdateAsync(Article item);
    Task<bool> DeleteAsync(string id);
    Task DeleteByAuthorAsync(string id);
    Task CreateAuthorAsync(string id);
    Task DeleteAuthorAsync(string id);
}