using Domain;

namespace Application.Interfaces.Messaging;

public interface IPricesGeneratedMessagePublisher
{
    Task<Guid?> PublishMessage(IEnumerable<Price> prices);
}
