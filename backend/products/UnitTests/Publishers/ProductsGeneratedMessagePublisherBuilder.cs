using Domain;
using MassTransit;
using Messaging.Messages;
using Messaging.Publishers;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Publishers;

internal class ProductsGeneratedMessagePublisherBuilder
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductsGeneratedMessagePublisher> _logger;
    private readonly ProductsGeneratedMessagePublisher _publisher;

    public ProductsGeneratedMessagePublisherBuilder()
    {
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _logger = Substitute.For<ILogger<ProductsGeneratedMessagePublisher>>();

        _publisher = new(_publishEndpoint, _logger);
    }

    public ProductsGeneratedMessagePublisherBuilder WithPublishedMessageId(Guid messageId)
    {
        _publishEndpoint.WhenForAnyArgs(p => p.Publish(new ProductsGenerated(Enumerable.Empty<Product>()), _ => { })).Do(x => _publisher.MessageId = messageId);

        return this;
    }

    public ProductsGeneratedMessagePublisher Build() => _publisher;
}
