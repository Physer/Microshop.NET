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

    internal ConsumeContext<ProductsGenerated>? _consumeContext;

    public ProductsGeneratedConsumerBuilder()
    {
        _logger = Substitute.For<ILogger<ProductsGeneratedConsumer>>();
        _consumeContext = Substitute.For<ConsumeContext<ProductsGenerated>>();
        _priceGenerator = Substitute.For<IPriceGenerator>();
    }

    public ProductsGeneratedConsumerBuilder WithoutConsumeContext()
    {
        _consumeContext = null;

        return this;
    }

    public ProductsGeneratedConsumerBuilder WithConsumerContextHavingMessageWithProducts(IEnumerable<Product> products)
    {
        _consumeContext!.Message.Returns(new ProductsGenerated(products));

        return this;
    }

    public ProductsGeneratedConsumerBuilder WithPriceGeneratorReturningPrices(IEnumerable<Product> inputProducts, IEnumerable<Price> pricesToReturn)
    {
        _priceGenerator.GeneratePrices(inputProducts).Returns(pricesToReturn);

        return this;
    }

    public ProductsGeneratedConsumer Build() => new(_logger, _priceGenerator);
}
