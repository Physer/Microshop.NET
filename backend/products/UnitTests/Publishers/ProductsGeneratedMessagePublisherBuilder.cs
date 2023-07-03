using MassTransit;
using Messaging.Publishers;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Publishers;

internal class ProductsGeneratedMessagePublisherBuilder
{
    internal IPublishEndpoint PublishEndpoint;
    internal ILogger<ProductsGeneratedMessagePublisher> Logger;

    public ProductsGeneratedMessagePublisherBuilder()
    {
        PublishEndpoint = Substitute.For<IPublishEndpoint>();
        Logger = Substitute.For<ILogger<ProductsGeneratedMessagePublisher>>();
    }

    public ProductsGeneratedMessagePublisher Build() => new(PublishEndpoint, Logger);
}
