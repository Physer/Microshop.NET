using Domain;

namespace Application.Interfaces.Messaging;

public interface IProductsGeneratedMessagePublisher
{
    Task PublishMessage(IEnumerable<Product> products);
}
