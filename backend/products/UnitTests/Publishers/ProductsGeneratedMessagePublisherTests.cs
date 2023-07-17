using AutoFixture.Xunit2;
using Domain;
using Messaging.Messages;
using NSubstitute;
using Xunit;

namespace UnitTests.Publishers;

public class ProductsGeneratedMessagePublisherTests
{
    [Theory]
    [AutoData]
    public async Task PublishMessage_CallsPublishEndpoint(IEnumerable<Product> products)
    {
        // Arrange
        var productsGeneratedMessagePublisherBuilder = new ProductsGeneratedMessagePublisherBuilder();
        var productsGeneratedMessagePublisher = productsGeneratedMessagePublisherBuilder.Build();

        // Act
        await productsGeneratedMessagePublisher.PublishMessage(products);

        // Assert
        await productsGeneratedMessagePublisherBuilder._publishEndpoint.Received(1).Publish(new ProductsGenerated(products));
    }
}
