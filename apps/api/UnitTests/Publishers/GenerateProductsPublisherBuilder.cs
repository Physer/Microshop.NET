using MassTransit;
using Messaging;
using NSubstitute;

namespace UnitTests.Publishers;

internal class GenerateProductsPublisherBuilder
{
    internal readonly IPublishEndpoint _publishEndpoint;

    public GenerateProductsPublisherBuilder() => _publishEndpoint = Substitute.For<IPublishEndpoint>();

    public GenerateProductsPublisher Build() => new(_publishEndpoint);
}
