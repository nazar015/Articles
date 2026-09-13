using MediatR;

namespace Users.Application.Commands.CreateAuthor;

public record CreateAuthorCommand(string FirstName, string LastName, string Email, string Password, decimal SubscriptionFee) : IRequest<string>;