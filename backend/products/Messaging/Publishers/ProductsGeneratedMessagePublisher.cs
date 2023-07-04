using Application.Exceptions;
using Application.Interfaces.Messaging;
using Domain;
using MassTransit;
using Messaging.Messages;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace Messaging.Publishers;

public class ProductsGeneratedMessagePublisher : IProductsGeneratedMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductsGeneratedMessagePublisher> _logger;

    internal Guid? _messageId;

    public ProductsGeneratedMessagePublisher(IPublishEndpoint publishEndpoint,
        ILogger<ProductsGeneratedMessagePublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
        _messageId = Guid.Empty;
    }

    public async Task<Guid?> PublishMessage(IEnumerable<Product> products)
    {
        _logger.LogInformation("Publishing message from {publisher}", nameof(ProductsGeneratedMessagePublisher));
        await _publishEndpoint.Publish(new ProductsGenerated(products), x =>
        {
            _messageId = x.MessageId;
        });

        if (_messageId == Guid.Empty)
            throw new MessagePublishingException($"Unable to publish a message using the {nameof(ProductsGeneratedMessagePublisher)}");
        return _messageId;
    }
}
