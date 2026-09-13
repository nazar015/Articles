using MediatR;
using Subscriptions.Application.Repositories;

namespace Subscriptions.Application.Commands.DeleteSubscription;

public class DeleteSubscriptionHandler(ISubscriptionRepository repository): IRequestHandler<DeleteSubscriptionCommand, bool>
{
    public async Task<bool> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var isDeleted = await repository.DeleteAsync(request.Id);
        return isDeleted;
    }
}