using MediatR;
using Subscriptions.Domain.Entities;

namespace Subscriptions.Application.Queries.GetSubscriptions;

public record GetSubscriptionsQuery(string SearchText): IRequest<IEnumerable<Subscription>>;