using MediatR;

namespace Subscriptions.Application.Commands.CreateSubscription;

public record CreateSubscriptionCommand(string ReaderId, string AuthorId): IRequest<string>;