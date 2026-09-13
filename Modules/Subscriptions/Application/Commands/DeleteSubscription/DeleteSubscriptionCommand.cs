using MediatR;

namespace Subscriptions.Application.Commands.DeleteSubscription;

public record DeleteSubscriptionCommand(string Id): IRequest<bool>;