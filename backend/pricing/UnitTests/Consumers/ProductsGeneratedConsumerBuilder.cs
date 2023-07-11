using Application.Interfaces.Generator;
using Domain;
using MassTransit;
using Messaging;
using Messaging.Messages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Consumers;

internal class ProductsGeneratedConsumerBuilder
{
    internal readonly ILogger<ProductsGeneratedConsumer> _logger;
    internal readonly IPriceGenerator _priceGenerator;

    internal ConsumeContext<ProductsGenerated> _consumeContext;

    public ProductsGeneratedConsumerBuilder()
    {
        _logger = Substitute.For<ILogger<ProductsGeneratedConsumer>>();
        _consumeContext = Substitute.For<ConsumeContext<ProductsGenerated>>();
        _priceGenerator = Substitute.For<IPriceGenerator>();
    }

    public ProductsGeneratedConsumerBuilder WithPriceGeneratorReturningPrices(IEnumerable<Price> prices)
    {
        _priceGenerator.GeneratePrices(850).Returns(prices);

        return this;
    }

    public ProductsGeneratedConsumer Build() => new(_logger, _priceGenerator);
}
