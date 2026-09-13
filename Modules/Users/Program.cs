using OpenApiUi;
using Users.Api.Endpoints;
using Users.Application.Repositories;
using Users.Infrastructure.Messaging;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddSwaggerGen(_ =>
{
    _.SwaggerDoc("v1", new()
    {
        Title = "User API",
        Version = "v1",
        Description = "ASP.NET Web API using Minimal APIs",
        Contact = new()
        {
            Name = "John Doe",
            Email = "john.doe@email.com"
        }
    });
});

builder.Services.AddServiceBus(builder.Configuration);
builder.Services.AddCosmosDbContainer(builder.Configuration);
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApiUi(config =>
    {
        config.OpenApiSpecPath = "/openapi/v1.json";
    });
    app.UseSwagger();
    app.UseSwaggerUI(_ =>
    {
        _.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
        _.RoutePrefix = "swagger";
        _.DocumentTitle = "User API Documentation";
    });
}

app.UseHttpsRedirection();
app.MapAuthorEndpoints();
app.MapReaderEndpoints();

app.Run();