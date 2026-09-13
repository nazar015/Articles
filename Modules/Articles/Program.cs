using Articles.Api.Endpoints;
using Articles.Application.Repositories;
using Articles.Infrastructure.Persistence;
using Articles.Infrastructure.Persistence.Repositories;
using Articles.Api.Middlewares;
using Articles.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddSwaggerGen(_ =>
{
    _.SwaggerDoc("v1", new()
    {
        Title = "Article API",
        Version = "v1",
        Description = "ASP.NET Web API using Minimal APIs",
        Contact = new()
        {
            Name = "John Doe",
            Email = "john.doe@email.com"
        }
    });
});

builder.Services.AddCosmosDbContainer(builder.Configuration);
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddHostedService<AuthorEventsConsumer>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(_ =>
    {
        _.SwaggerEndpoint("/swagger/v1/swagger.json", "Article API v1");
        _.RoutePrefix = "swagger";
        _.DocumentTitle = "Article API Documentation";
    });
}

app.UseHttpsRedirection();
app.MapArticleEndpoints();

app.Run();