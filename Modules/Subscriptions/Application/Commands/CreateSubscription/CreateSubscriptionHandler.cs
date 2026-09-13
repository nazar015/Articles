using MediatR;
using Subscriptions.Application.Repositories;
using Subscriptions.Domain.Entities;

namespace Subscriptions.Application.Commands.CreateSubscription;

public class CreateSubscriptionHandler(ISubscriptionRepository repository): IRequestHandler<CreateSubscriptionCommand, string>
{
    /// <returns>Id of a created entity.</returns>
    public async Task<string> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = new Subscription()
        {
            Id = Guid.NewGuid().ToString(),
            ReaderId = request.ReaderId,
            AuthorId = request.AuthorId,
        };

        await repository.CreateAsync(subscription);
        return subscription.Id;
    }
}