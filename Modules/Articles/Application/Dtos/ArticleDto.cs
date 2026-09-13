namespace Articles.Application.Dtos;

public record ArticleDto(string Id, string Image, string Title, decimal Price, string AuthorId, string CategoryId);