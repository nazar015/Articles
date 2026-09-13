using MediatR;
using Subscriptions.Application.Repositories;
using Subscriptions.Domain.Entities;

namespace Subscriptions.Application.Queries.GetSubscriptions;

public class GetSubscriptionsHandler(ISubscriptionRepository repository): IRequestHandler<GetSubscriptionsQuery, IEnumerable<Subscription>>
{
    public async Task<IEnumerable<Subscription>> Handle(GetSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var results = await repository.GetAsync(request.SearchText);
        return  results;
    }
}