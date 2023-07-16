using Application.Interfaces.Indexing;
using Domain;
using MassTransit;
using Messaging;
using Messaging.Messages;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTests.Builders;

internal class ProductsGeneratedConsumerBuilder
{
    internal readonly IIndexingService _indexingService;
    internal readonly ILogger<ProductsGeneratedConsumer> _logger;

    internal ConsumeContext<ProductsGenerated>? _consumeContext;

    public ProductsGeneratedConsumerBuilder()
    {
        _indexingService = Substitute.For<IIndexingService>();
        _logger = Substitute.For<ILogger<ProductsGeneratedConsumer>>();
        _consumeContext = Substitute.For<ConsumeContext<ProductsGenerated>>();
    }

    public ProductsGeneratedConsumerBuilder WithReceivingProducts(IEnumerable<Product> products)
    {
        _consumeContext!.Message.Returns(new ProductsGenerated(products));

        return this;
    }

    public ProductsGeneratedConsumerBuilder WithMessage(Guid messageId)
    {
        _consumeContext!.MessageId.Returns(messageId);

        return this;
    }

    public ProductsGeneratedConsumerBuilder WithoutConsumeContext()
    {
        _consumeContext = null;

        return this;
    }

    public ProductsGeneratedConsumer Build() => new(_indexingService, _logger);
}
