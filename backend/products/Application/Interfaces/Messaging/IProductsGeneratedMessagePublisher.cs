using Domain;

namespace Application.Interfaces.Messaging;

public interface IProductsGeneratedMessagePublisher
{
    Task<Guid?> PublishMessage(IEnumerable<Product> products);
}
