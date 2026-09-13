using Subscriptions.Api.Endpoints;
using Subscriptions.Application.Repositories;
using Subscriptions.Infrastructure.Messaging;
using Subscriptions.Infrastructure.Persistence.Repositories;
using Subscriptions.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddSwaggerGen(_ =>
{
    _.SwaggerDoc("v1", new()
    {
        Title = "Subscription API",
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
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddHostedService<UserEventsConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(_ =>
    {
        _.SwaggerEndpoint("/swagger/v1/swagger.json", "Subscription API v1");
        _.RoutePrefix = "swagger";
        _.DocumentTitle = "Subscription API Documentation";
    });
}

app.UseHttpsRedirection();
app.MapSubscriptionEndpoints();

app.Run();