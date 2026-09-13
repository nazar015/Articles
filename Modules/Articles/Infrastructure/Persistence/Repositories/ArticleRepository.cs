using Articles.Application.Repositories;
using Articles.Domain.Entities;
using Shared.Exceptions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using Shared.Persistence;

namespace Articles.Infrastructure.Persistence.Repositories;

public class ArticleRepository : IArticleRepository
{
    private readonly Container _articles;
    private readonly Container _authors;
    private readonly Container _categories;

    public ArticleRepository(CosmosClient cosmosClient, IOptions<CosmosDbOptions> options)
    {
        _articles = cosmosClient.GetContainer(options.Value.DatabaseName, options.Value.Containers["Articles"]);
        _authors = cosmosClient.GetContainer(options.Value.DatabaseName, options.Value.Containers["Authors"]);
        _categories = cosmosClient.GetContainer(options.Value.DatabaseName, options.Value.Containers["Categories"]);
    }
    public async Task<Article> GetAsync(string id)
    {
        var article = await _articles.ReadItemAsync<Article>(id, new PartitionKey(id));

        if (article == null)
        {
            throw new EntityNotFoundException(nameof(Article), id);
        }
        
        return article.Resource;
    }

    public async Task<IEnumerable<Article>> GetAsync(string authorId, string categoryId, string searchText)
    {
        IQueryable<Article> queryable = _articles.GetItemLinqQueryable<Article>();

        if (!string.IsNullOrEmpty(authorId))
        {
            queryable = queryable.Where(a => a.AuthorId == authorId);
        }
        if (!string.IsNullOrEmpty(categoryId))
        {
            queryable = queryable.Where(a => a.CategoryId == categoryId);
        }
        if (!string.IsNullOrEmpty(searchText))
        {
            queryable = queryable.Where(a => a.Title.Contains(searchText));
        }

        using var iterator = queryable.ToFeedIterator();

        var results = new List<Article>();

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response.Resource);
        }

        return results;
    }

    public async Task<Article> CreateAsync(Article item)
    {
        await ValidateArticleAsync(item);
        
        var result = await _articles.CreateItemAsync(item, new PartitionKey(item.Id));
        return result.Resource;
    }

    public async Task<Article> UpdateAsync(Article item)
    {
        await ValidateArticleAsync(item);
        
        var result = await _articles.UpsertItemAsync(item, new PartitionKey(item.Id));
        return result.Resource;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var deleted = await _articles.DeleteItemAsync<Article>(id, new PartitionKey(id));
        return (int)deleted.StatusCode == 200 || (int)deleted.StatusCode == 204;
    }
    
    public async Task DeleteByAuthorAsync(string id)
    {
        var articles = _articles
            .GetItemLinqQueryable<Article>(allowSynchronousQueryExecution: false)
            .Where(s => s.AuthorId == id)
            .ToFeedIterator();

        while (articles.HasMoreResults)
        {
            foreach (var item in await articles.ReadNextAsync())
            {
                await _articles.DeleteItemAsync<Article>(
                    item.Id,
                    new PartitionKey(item.Id));
            }
        }
    }

    public async Task CreateAuthorAsync(string id)
    {
        var item = new Author() { Id = id, CreatedAt = DateTime.UtcNow };
        await _authors.CreateItemAsync(item, new PartitionKey(item.Id));
    }
    
    public async Task DeleteAuthorAsync(string id)
    {
        await _authors.DeleteItemAsync<Author>(id, new PartitionKey(id));
    }

    private async Task ValidateArticleAsync(Article article)
    {
        List<string> authors = [];
        using var iterator = _authors.GetItemLinqQueryable<Author>().Select(_ => _.Id).ToFeedIterator();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            authors.AddRange(response.Resource);
        }
        
        List<string> categories = [];
        using var iteratorForCategories = _categories.GetItemLinqQueryable<Category>().Select(_ => _.Id).ToFeedIterator();
        while (iteratorForCategories.HasMoreResults)
        {
            var response = await iteratorForCategories.ReadNextAsync();
            categories.AddRange(response.Resource);
        }

        var errors = new Dictionary<string, string>();

        if (authors.All(id => id != article.AuthorId))
        {
            errors.Add("author id", "no corresponding authors were found");
        }

        if (categories.All(id => id != article.CategoryId))
        {
            errors.Add("category id", "no corresponding categories were found");
        }

        if (errors.Any())
        {
            throw new ValidationException(nameof(Author), errors);
        }
    }
}
