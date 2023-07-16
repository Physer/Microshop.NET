using Application.Models;
using AutoFixture.Xunit2;
using Domain;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UnitTests.Builders;
using Xunit;

namespace UnitTests;

public class PricesGeneratedConsumerTests
{
    [Theory]
    [AutoData]
    public async Task Consume_WithMessage_CallsLoggerAndIndexingServiceWithPrices(IEnumerable<Price> prices)
    {
        // Arrange
        var pricesGeneratedConsumerBuilder = new PricesGeneratedConsumerBuilder()
            .WithReceivingPrices(prices)
            .WithMessage(Guid.NewGuid());
        var pricesGeneratedConsumer = pricesGeneratedConsumerBuilder.Build();

        // Act
        await pricesGeneratedConsumer.Consume(pricesGeneratedConsumerBuilder._consumeContext!);

        // Assert
        await pricesGeneratedConsumerBuilder._indexingService.Received(1).IndexAsync<Price, IndexableProduct>(prices);
        pricesGeneratedConsumerBuilder._logger.ReceivedWithAnyArgs(1).LogInformation("Logging with argument: {argument}", new[] { "argument" });
    }

    [Fact]
    public async Task Consumer_WithoutValidMessageData_CallsLoggerAndIndexingServiceWithNull()
    {
        // Arrange
        var pricesGeneratedConsumerBuilder = new PricesGeneratedConsumerBuilder();
        var pricesGeneratedConsumer = pricesGeneratedConsumerBuilder.Build();

        // Act
        await pricesGeneratedConsumer.Consume(pricesGeneratedConsumerBuilder._consumeContext!);

        // Assert
        await pricesGeneratedConsumerBuilder._indexingService.Received(1).IndexAsync<Price, IndexableProduct>(null);
        pricesGeneratedConsumerBuilder._logger.ReceivedWithAnyArgs(1).LogInformation("Logging with argument: {argument}", new[] { "argument" });
    }

    [Fact]
    public async Task Consumer_WithoutConsumeContext_CallsLoggerAndIndexingServiceWithNull()
    {
        // Arrange
        var pricesGeneratedConsumerBuilder = new PricesGeneratedConsumerBuilder()
            .WithoutConsumeContext();
        var pricesGeneratedConsumer = pricesGeneratedConsumerBuilder.Build();

        // Act
        await pricesGeneratedConsumer.Consume(pricesGeneratedConsumerBuilder._consumeContext!);

        // Assert
        await pricesGeneratedConsumerBuilder._indexingService.Received(1).IndexAsync<Price, IndexableProduct>(null);
        pricesGeneratedConsumerBuilder._logger.ReceivedWithAnyArgs(1).LogInformation("Logging with argument: {argument}", new[] { "argument" });
    }
}
