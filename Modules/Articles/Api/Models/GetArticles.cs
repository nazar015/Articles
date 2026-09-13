using Microsoft.AspNetCore.Mvc;

namespace Articles.Api.Models;

public record GetArticles([FromQuery] string? AuthorId, [FromQuery] string? CategoryId,  [FromQuery] string? SearchText);