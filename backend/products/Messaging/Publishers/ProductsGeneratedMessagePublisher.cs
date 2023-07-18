using Application.Interfaces.Messaging;
using Domain;
using MassTransit;
using Messaging.Messages;

namespace Messaging.Publishers;

public class ProductsGeneratedMessagePublisher : IProductsGeneratedMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductsGeneratedMessagePublisher(IPublishEndpoint publishEndpoint) => _publishEndpoint = publishEndpoint;

    public async Task PublishMessage(IEnumerable<Product> products) => await _publishEndpoint.Publish(new ProductsGenerated(products));
}
