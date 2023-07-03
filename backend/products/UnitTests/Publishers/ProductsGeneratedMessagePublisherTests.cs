using Domain;
using Messaging.Messages;
using NSubstitute;
using Xunit;

namespace UnitTests.Publishers;

public class ProductsGeneratedMessagePublisherTests
{
    [Fact]
    public async Task PublishMessage_CallsDependencies()
    {
        // Arrange
        var productsGeneratedMessagePublisherBuilder = new ProductsGeneratedMessagePublisherBuilder();
        var productsGeneratedMessagePublisher = productsGeneratedMessagePublisherBuilder.Build();

        // Act
        await productsGeneratedMessagePublisher.PublishMessage(Enumerable.Empty<Product>());

        // Assert
        productsGeneratedMessagePublisherBuilder.Logger.Received(1);
        await productsGeneratedMessagePublisherBuilder.PublishEndpoint.Received(1).Publish(new ProductsGenerated());
    }
}
