using MassTransit;
using Messaging.Publishers;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Publishers;

internal class ProductsGeneratedMessagePublisherBuilder
{
    internal readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductsGeneratedMessagePublisher> _logger;
    private readonly ProductsGeneratedMessagePublisher _publisher;

    public ProductsGeneratedMessagePublisherBuilder()
    {
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _logger = Substitute.For<ILogger<ProductsGeneratedMessagePublisher>>();

        _publisher = new(_publishEndpoint, _logger);
    }

    public ProductsGeneratedMessagePublisher Build() => _publisher;
}
