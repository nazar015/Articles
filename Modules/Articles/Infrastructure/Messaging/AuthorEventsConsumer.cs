using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Shared.Messaging.Events;
using Articles.Application.Repositories;

namespace Articles.Infrastructure.Messaging;

public class AuthorEventsConsumer : BackgroundService
{
    private readonly ServiceBusClient _client;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AuthorEventsConsumer> _logger;

    private ServiceBusProcessor? _processor;
    
    private const string TopicName = "user-events";
    private const string SubscriptionName = "subscriptions-module";

    public AuthorEventsConsumer(
        ServiceBusClient client,
        IServiceScopeFactory scopeFactory,
        ILogger<AuthorEventsConsumer> logger)
    {
        _client = client;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor = _client.CreateProcessor(TopicName, SubscriptionName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 5,
            PrefetchCount = 10
        });

        _processor.ProcessMessageAsync += ProcessMessageAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;

        await _processor.StartProcessingAsync(stoppingToken);

        _logger.LogInformation("AuthorEventsConsumer started. Listening on {Topic}/{Subscription}",
            TopicName, SubscriptionName);
        
        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            // expected on shutdown
        }
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var subject = args.Message.Subject;
        var body = args.Message.Body.ToString();

        _logger.LogInformation("Received event {Subject}: {Body}", subject, body);

        try
        {
            using var scope = _scopeFactory.CreateScope();
            
            var repository = scope.ServiceProvider
                .GetRequiredService<IArticleRepository>();

            switch (subject)
            {
                case "AuthorDeleted":
                    var authorDeleted = JsonSerializer.Deserialize<AuthorDeleted>(body)
                        ?? throw new InvalidOperationException("Invalid AuthorDeleted payload");

                    await repository.DeleteByAuthorAsync(authorDeleted.UserId);
                    await repository.DeleteAuthorAsync(authorDeleted.UserId);
                    break;
                
                case "AuthorCreated":
                    var authorCreated = JsonSerializer.Deserialize<AuthorCreated>(body)
                                        ?? throw new InvalidOperationException("Invalid AuthorDeleted payload");

                    await repository.CreateAuthorAsync(authorCreated.UserId);
                    break;

                default:
                    _logger.LogWarning("Unknown event subject: {Subject}. Message will be completed.", subject);
                    break;
            }
            
            await args.CompleteMessageAsync(args.Message, args.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message {MessageId} with subject {Subject}",
                args.Message.MessageId, subject);

            // for retry
            await args.AbandonMessageAsync(args.Message, cancellationToken: args.CancellationToken);
        }
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception,
            "Service Bus error. Source: {ErrorSource}, Entity: {EntityPath}",
            args.ErrorSource, args.EntityPath);

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_processor is not null)
        {
            await _processor.StopProcessingAsync(cancellationToken);
            await _processor.DisposeAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}