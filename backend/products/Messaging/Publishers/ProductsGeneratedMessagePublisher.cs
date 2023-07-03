using Application.Interfaces.Messaging;
using MassTransit;
using Messaging.Messages;
using Microsoft.Extensions.Logging;

namespace Messaging.Publishers;

public class ProductsGeneratedMessagePublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductsGeneratedMessagePublisher> _logger;

    public ProductsGeneratedMessagePublisher(IPublishEndpoint publishEndpoint,
        ILogger<ProductsGeneratedMessagePublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishMessage()
    {
        _logger.LogInformation("Publishing message from {publisher}", nameof(ProductsGeneratedMessagePublisher));
        await _publishEndpoint.Publish(new ProductsGenerated());
    }
}
