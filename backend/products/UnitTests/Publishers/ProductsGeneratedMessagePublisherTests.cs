using Application.Exceptions;
using Domain;
using FluentAssertions;
using Xunit;

namespace UnitTests.Publishers;

public class ProductsGeneratedMessagePublisherTests
{
    [Fact]
    public async Task PublishMessage_WhenMessageIsPublished_ReturnsMessageId()
    {
        // Arrange
        var expectedMessageId = Guid.NewGuid();
        var productsGeneratedMessagePublisherBuilder = new ProductsGeneratedMessagePublisherBuilder();
        var productsGeneratedMessagePublisher = productsGeneratedMessagePublisherBuilder
            .WithPublishedMessageId(expectedMessageId)
            .Build();

        // Act
        var messageId = await productsGeneratedMessagePublisher.PublishMessage(Enumerable.Empty<Product>());

        // Assert
        messageId.Should().Be(expectedMessageId);
    }

    [Fact]
    public async Task PublishMessage_WhenMessageIsNotPublishedSuccesfully_ThrowsPublishException()
    {
        // Arrange
        var productsGeneratedMessagePublisherBuilder = new ProductsGeneratedMessagePublisherBuilder();
        var productsGeneratedMessagePublisher = productsGeneratedMessagePublisherBuilder.Build();

        // Act
        var exception = await Record.ExceptionAsync(() => productsGeneratedMessagePublisher.PublishMessage(Enumerable.Empty<Product>()));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<MessagePublishingException>();
    }
}
